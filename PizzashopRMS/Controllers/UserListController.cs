// using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BusinessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace PizzashopRMS.Controllers;

public class UserListController : Controller
{
    private readonly IUserList _userListRepository;

    public UserListController(IUserList userListRepository)
    {
        _userListRepository = userListRepository;
    }

    [HttpGet]
    public async Task<IActionResult> UserListView(int PageSize = 5, int PageNumber = 1, string sortBy = "name", string sortOrder = "asc", string SearchKey = "")
    {
        // Get data from repository (returns a tuple)
        var (users, count, pageSize, pageNumber, sortColumn, sortDirection, searchKey) = await _userListRepository.GetUsers(PageSize, PageNumber, sortBy, sortOrder, SearchKey);

        // Store metadata in ViewData (converted to correct types)
        ViewData["sortBy"] = sortColumn;
        ViewData["sortOrder"] = sortDirection;
        ViewData["PageSize"] = pageSize;
        ViewData["PageNumber"] = pageNumber;
        ViewData["SearchKey"] = searchKey;
        ViewData["Count"] = count;  // Total user count for pagination

        // Pass only the user list (List<UserListViewModel>) to the View
        return View(users);
    }

    [HttpGet]
    public IActionResult AddUserView()
    {
        var model = new AddUserViewModel
        {
            Roles = _userListRepository.GetRoles(),
            Countries = _userListRepository.GetCountries(),
            States = new List<State>(),
            Cities = new List<City>()
        };

        return View(model);
    }
    // Fetch states based on selected country
    [HttpGet]
    public JsonResult GetStates(int countryId)
    {
        var states = _userListRepository.GetStatesByCountry(countryId);
        return Json(states);
    }

    // Fetch cities based on selected state
    [HttpGet]
    public JsonResult GetCities(int stateId)
    {
        var cities = _userListRepository.GetCitiesByState(stateId);
        return Json(cities);
    }

    private string GetUserEmailFromToken()
    {
        var token = Request.Cookies["JWTLogin"];
        if (string.IsNullOrEmpty(token)) return "";

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        return jwtToken.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name)?.Value ?? "";
    }
    [HttpPost]
    public async Task<IActionResult> AddUserViewAsync(AddUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        string email = GetUserEmailFromToken();
        bool is_useradded = await _userListRepository.AddUser(model, email);
        if (!is_useradded)
        {
            TempData["error"] = "This email already exist.";
            return RedirectToAction("UserListView"); // Redirect and show error message
        }
        string callbackUrl = Url.ActionLink("UserListView", "UserList");
        string newEmail = model.Email;
        bool isEmailSent = await _userListRepository.AddUserEmail(newEmail, callbackUrl);
        if (isEmailSent)
        {
            TempData["success"] = "A password reset link has been sent to your email.";
        }
        else
        {
            TempData["error"] = "Email not found!";
        }

        return RedirectToAction("UserListView");
    }

    [HttpGet]
    public async Task<IActionResult> EditUserView(int userId)
    {

        var model = await _userListRepository.GetUserProfileDetailsAsync(userId);
        if (model == null) return NotFound("User Not Found");
        model.Roles = _userListRepository.GetRoles();
        model.Countries = _userListRepository.GetCountries();
        model.States = _userListRepository.GetStatesByCountry(model.Countryid);
        model.Cities = _userListRepository.GetCitiesByState(model.Stateid);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUserProfileView(EditUserViewModel model)
    {
        if (!ModelState.IsValid) return RedirectToAction("UserListView");

        var success = await _userListRepository.EditUserProfileDetailsAsync(model);
        if (success) return RedirectToAction("UserListView");

        ModelState.AddModelError("", "Failed to update user.");
        return View("UserListView", model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await _userListRepository.DeleteUser(userId);
        return RedirectToAction("UserListView");
    }

}