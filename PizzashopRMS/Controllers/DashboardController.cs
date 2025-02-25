using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PizzashopRMS.Controllers;

// [Authorize (Roles = "admin")]
public class DashboardController : Controller
{
   

        public IActionResult DashboardView()
        {
            return View(); 
        }
}
