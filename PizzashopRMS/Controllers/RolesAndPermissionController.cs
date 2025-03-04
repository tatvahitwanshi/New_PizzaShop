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


    [HttpGet]
    public async Task<IActionResult> PermissionView(int roleid)
    {
        var role = await _roleAndPermission.GetRoleByIdAsync(roleid);
        var permissions = await _roleAndPermission.GetPermissionsByRoleIdAsync(roleid);

        var viewModel = new PermissionViewModel
        {
            Roleid = roleid,
            CreatedBy = "Admin",
            CreatedDate = DateTime.Now,
            IsEnable = true
        };

        ViewBag.RoleName = role?.Rolename;
        ViewBag.Permissions = permissions; // Pass permissions to the view

        return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> UpdatePermissionView(PermissionUpdateRequest model)
    {
        try
        {
            bool isUpdated = await _roleAndPermission.UpdatePermissionsAsync(model);

            if (isUpdated)
            {
                TempData["success"] = "Permissions updated successfully!";
            }
            else
            {
                TempData["error"] = "Failed to update permissions!";
            }
        }
        catch (Exception ex)
        {
            TempData["error"] = "An error occurred while updating permissions: " + ex.Message;
        }

        return RedirectToAction("PermissionView", new { roleid = model.RoleId });
    }


}
