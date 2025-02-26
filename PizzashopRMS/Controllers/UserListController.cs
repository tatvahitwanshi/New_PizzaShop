using BusinessLayer.Interface;
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

        // POST: Handle user search
        [HttpPost]
        public IActionResult UserListView(string searchQuery, string sortBy = "name", string sortOrder = "asc")
        {
            var users = _userListRepository.GetUsers(sortBy, sortOrder);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                users = users.FindAll(u => 
                    u.Firstname?.ToLower().Contains(searchQuery.ToLower()) == true ||
                    u.Email?.ToLower().Contains(searchQuery.ToLower()) == true);
            }

            ViewData["SortBy"] = sortBy;
            ViewData["SortOrder"] = sortOrder;
            ViewData["SearchQuery"] = searchQuery;

            return View(users);
        }
}