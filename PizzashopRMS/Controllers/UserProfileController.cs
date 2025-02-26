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
            string email = GetUserEmailFromToken();
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "Login");

            var model = await _userProfile.GetUserProfileAsync(email);
            if (model == null) return NotFound("User Not Found");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserProfileView(UserProfileViewModel model)
        {
            if (model.Username == null || model.Countryid== null || model.Cityid==null || model.Stateid==null) return RedirectToAction("UserProfileView");

            var success = await _userProfile.UpdateUserProfileAsync(model);
            if (success) return RedirectToAction("UserProfileView");

            ModelState.AddModelError("", "Failed to update profile.");
            return RedirectToAction("UserProfileView");
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
            if (!ModelState.IsValid) return View("ChangePasswordView");

            string email = GetUserEmailFromToken();
            var success = await _userProfile.ChangePasswordAsync(email, model.OldPassword, model.ConfirmNewPassword);
            TempData["success"] = "Password changed successful";
            if (success) return RedirectToAction("LoginView", "Login");

            ModelState.AddModelError("", "Password change failed.");
            return View("ChangePasswordView");
        }
    }
}
