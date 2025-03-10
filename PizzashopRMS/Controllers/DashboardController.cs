using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzashopRMS.Helpers;

namespace PizzashopRMS.Controllers;

// dashboard view and applies authorization for admin users.
[CustomAuthorise(new string[] { "admin" })]
public class DashboardController : Controller
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult DashboardView()
    {

        return View();
    }
}
