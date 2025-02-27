using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace PizzashopRMS.Controllers;

public class RolesAndPermissionController :Controller
{
 
    public IActionResult RoleView(){

        return View();
    }


    public IActionResult PermissionView()
    {
        return View();
    }
}
