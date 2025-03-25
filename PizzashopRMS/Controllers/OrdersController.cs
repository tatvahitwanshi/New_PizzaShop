using Microsoft.AspNetCore.Mvc;

namespace PizzashopRMS.Controllers;

public class OrdersController : Controller
{
    public IActionResult OrdersView(){
        return View();
    }
}
