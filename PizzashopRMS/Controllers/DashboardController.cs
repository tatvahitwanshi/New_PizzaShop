using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzashopRMS.Helpers;

namespace PizzashopRMS.Controllers;
[CustomAuthorise(new string[]{"admin"})]

// [Authorize (Roles = "admin")]
public class DashboardController : Controller
{
        public IActionResult DashboardView()
        {
            
            return View(); 
        }
}
