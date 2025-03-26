using BusinessLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using PizzashopRMS.Helpers;

namespace PizzashopRMS.Controllers;

[CustomAuthorise(new string[] { "admin" })]
public class OrdersController : Controller
{
    private readonly IOrders _orders;

    public OrdersController(IOrders orders)
    {
        _orders = orders;
    }

    public IActionResult OrdersView()
    {
        OrdersViewModel model= new OrdersViewModel{
            OrderStatus=_orders.GetOrderStatus(),
            OrderPage=_orders.GetOrders()
        };
              
        return View(model);
    }
    
    [HttpGet]
    public IActionResult OrderListTable(int Pagenumber = 1, int Pagesize = 10, int OrderStatusId = 0, string Searchkey = "", string sortDr = "asc", string sortCol = "OrderNo", string lastDays = "All Time",  string startDate = "", string endDate = "")
    {
        
        DateTime? startDateTime = string.IsNullOrEmpty(startDate) ? null : DateTime.Parse(startDate);
        DateTime? endDateTime = string.IsNullOrEmpty(endDate) ? null : DateTime.Parse(endDate);

        OrdersViewModel model = new OrdersViewModel{
            OrderStatus = _orders.GetOrderStatus(),
            OrderPage = _orders.GetOrders(Pagenumber, Pagesize, OrderStatusId, Searchkey, sortDr, sortCol, lastDays, startDateTime, endDateTime)
        };
        return PartialView("_PartialOrderList",model);
    }
}


