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


    // GET: Retrieve the user list
    [HttpGet]
    public IActionResult UserListView(string sortBy = "name", string sortOrder = "asc")
    {
        var users = _userListRepository.GetUsers(sortBy, sortOrder);
        ViewData["SortBy"] = sortBy;
        ViewData["SortOrder"] = sortOrder;

        return View(users);
    }

    // // POST: Handle user search
    // [HttpPost]
    // public IActionResult UserListView(string searchQuery, string sortBy = "name", string sortOrder = "asc")
    // {
    //     var users = _userListRepository.GetUsers(sortBy, sortOrder);

    //     if (!string.IsNullOrEmpty(searchQuery))
    //     {
    //         users = users.FindAll(u =>
    //             u.Firstname?.ToLower().Contains(searchQuery.ToLower()) == true ||
    //             u.Email?.ToLower().Contains(searchQuery.ToLower()) == true);
    //     }

    //     ViewData["SortBy"] = sortBy;
    //     ViewData["SortOrder"] = sortOrder;
    //     ViewData["SearchQuery"] = searchQuery;

    //     return View(users);
    // }

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
    public IActionResult AddUserView(AddUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        string email = GetUserEmailFromToken();
        _userListRepository.AddUser(model,email);

        return RedirectToAction("UserListView");
    }

    [HttpGet]
    public IActionResult EditUserView(int userId)
    { 
        Console.WriteLine(userId);
    
        return View();
    }

}