using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzashopRMS.Helpers;

namespace PizzashopRMS.Controllers;
[CustomAuthorise(new string[] { "admin" })]

public class DashboardController : Controller
{
    // [CustomAuthorise(new string[] { "admin" })]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult DashboardView()
    {

        return View();
    }
}
