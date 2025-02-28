using BusinessLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace PizzashopRMS.Controllers;

public class RolesAndPermissionController : Controller
{
    private readonly IRolesAndPermission _roleAndPermission;
    public RolesAndPermissionController(IRolesAndPermission rolesAndPermission)
    {
        _roleAndPermission = rolesAndPermission;
    }

    [HttpGet]
    public async Task<IActionResult> RoleView()
    {
        var roles = await _roleAndPermission.GetRolesAsync();
        return View(roles);
    }

    public IActionResult PermissionView()
    {
        return View();
    }
}
