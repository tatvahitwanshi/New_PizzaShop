using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace PizzaShopApp.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserProfile _userProfile;

        public UserProfileController(IUserProfile userProfile)
        {
            _userProfile = userProfile;
        }

        private string GetUserEmailFromToken()
        {
            var token = Request.Cookies["JWTLogin"];
            if (string.IsNullOrEmpty(token)) return "";

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name)?.Value ?? "";
        }

        [HttpGet]
        public async Task<IActionResult> UserProfileView()
        {
            var token = Request.Cookies["JWTLogin"];
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var role = jwtToken.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value ?? "";
            ViewData["Role"] = role;
            string email = GetUserEmailFromToken();
            if (string.IsNullOrEmpty(email)) return RedirectToAction("LoginView", "Login");

            var model = await _userProfile.GetUserProfileAsync(email);
            if (model == null) return NotFound("User Not Found");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserProfileView(UserProfileViewModel model)
        {
            if (model.Username == null || model.Countryid == 0 || model.Cityid == 0 || model.Stateid == 0)
            {
                if (model.Countryid == 0)
                {
                    TempData["error"] = "Please Select the country";
                    return RedirectToAction("UserProfileView");
                }
                if (model.Stateid == 0)
                {
                    TempData["error"] = "Please Select the state";
                    return RedirectToAction("UserProfileView");

                }
                if (model.Stateid == 0)
                {
                    TempData["error"] = "Please Select the city";
                    return RedirectToAction("UserProfileView");

                }

            }
            // if(!ModelState.IsValid) return RedirectToAction("UserProfileView");

            var success = await _userProfile.UpdateUserProfileAsync(model);
            if (success)
            {
                TempData["success"] = "User Updated successfully";
                return RedirectToAction("UserProfileView");
            }
            else
            {
                TempData["error"] = "User Updated successfully";
                ModelState.AddModelError("", "Failed to update profile.");
                return RedirectToAction("UserProfileView");
            }


        }

        [HttpGet]
        public async Task<IActionResult> GetStates(int countryId)
        {
            var states = await _userProfile.GetStatesAsync(countryId);
            return Json(states.Select(s => new { stateId = s.Stateid, stateName = s.Statename }));
        }

        [HttpGet]
        public async Task<IActionResult> GetCities(int stateId)
        {
            var cities = await _userProfile.GetCitiesAsync(stateId);
            return Json(cities.Select(c => new { cityId = c.Cityid, cityName = c.Cityname }));
        }

        [HttpGet]
        public IActionResult ChangePasswordView()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Enter the password in proper formate";
                return View("ChangePasswordView");
            }

            string email = GetUserEmailFromToken();
            var success = await _userProfile.ChangePasswordAsync(email, model.OldPassword, model.ConfirmNewPassword);
            
            if (success)
            {
                TempData["success"] = "Password changed successful";
                return RedirectToAction("LoginView", "Login");

            }
            else{
                TempData["error"] = "Old Password does not match";
                return View("ChangePasswordView");

            }
           
            
        }
    }
}
