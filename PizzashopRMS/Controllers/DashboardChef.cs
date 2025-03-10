using Microsoft.AspNetCore.Mvc;
using PizzashopRMS.Helpers;

namespace PizzashopRMS.Controllers;

[CustomAuthorise(new string[] { "chef" })]
public class DashboardChef: Controller
{
    
   
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult DashboardChefView()
    {
        return View();
    }
    
    
   
}
