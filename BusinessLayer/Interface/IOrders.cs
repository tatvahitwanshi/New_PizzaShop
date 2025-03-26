using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;

namespace BusinessLayer.Interface;

public interface IOrders
{
    public OrderPage GetOrders(int pageNumber = 1, int pageSize = 5, int orderStatusId = 0, string searchKey = "", string sortDr = "asc", string sortCol = "OrderNo", string lastDays = "All Time", DateTime? startDate = null, DateTime? endDate = null);
    List<Orderstatus> GetOrderStatus();

}
