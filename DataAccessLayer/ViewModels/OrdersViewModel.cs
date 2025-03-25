namespace DataAccessLayer.ViewModels;

public class OrdersViewModel
{
    List<OrderList> OrderLists {get; set;}= new List<OrderList>();
}



public class OrderList
{
    public int Orderid {get; set;}

    public string? Customername {get; set;}

    public string? Status {get; set;}

    public string? PaymentMode {get; set;}

    public short? Rating { get; set; }

    public decimal? Totalamount { get; set; }

}
