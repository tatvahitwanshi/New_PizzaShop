using BusinessLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace PizzashopRMS.Controllers;

public class RolesAndPermissionController : Controller
{
    private readonly IRolesAndPermission _roleAndPermission;
    private readonly ILogger<RolesAndPermissionController> _logger;

    public RolesAndPermissionController(IRolesAndPermission rolesAndPermission, ILogger<RolesAndPermissionController> logger)
    {
        _roleAndPermission = rolesAndPermission;
        _logger = logger;
    }

    // Fetch and display all roles
    [HttpGet]
    public async Task<IActionResult> RoleView()
    {
        var roles = await _roleAndPermission.GetRolesAsync();
        return View(roles);
    }

    // Fetch and display permissions for a given role
    [HttpGet]
    public async Task<IActionResult> PermissionView(int roleid)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching permissions for RoleId {roleid}");
            TempData["error"] = "An error occurred while fetching permissions.";
            return RedirectToAction("RoleView");
        }


    }

    // Update permissions for a role
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
            _logger.LogError(ex, "Error while updating permissions.");
            TempData["error"] = "An error occurred while updating permissions: " + ex.Message;
        }

        return RedirectToAction("PermissionView", new { roleid = model.RoleId });
    }


}
