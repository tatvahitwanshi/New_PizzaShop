using System.Drawing;
using BusinessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace BusinessLayer.Repository;

public class OrdersRepository : IOrders
{
    private readonly PizzaShopContext _db;
    public OrdersRepository(PizzaShopContext db)
    {
        _db = db;

    }
    public OrderPage GetOrders(int pageNumber = 1, int pageSize = 5, int orderStatusId = 0, string searchKey = "", string sortDr = "asc", string sortCol = "OrderNo", string lastDays = "All Time", DateTime? startDate = null, DateTime? endDate = null)
    {
        OrderPage orders = new OrderPage();
        var query = from o in _db.Orders
                    join c in _db.Customers on o.Customerid equals c.Customerid
                    join os in _db.Orderstatuses on o.Orderstatusid equals os.Orderstatusid
                    join ps in _db.Payments on o.Paymentid equals ps.Paymentid
                    where o.Orderid.ToString().ToLower().Contains(searchKey.ToLower()) ||
                          c.Customername.ToLower().Contains(searchKey.ToLower()) ||
                          os.Statusname.ToLower().Contains(searchKey.ToLower()) ||
                          ps.Paymentmode.ToLower().Contains(searchKey.ToLower())
                    orderby o.Orderid
                    select new
                    {
                        order = o,
                        customer = c,
                        orderstatus = os,
                        paymentStatus = ps
                    };
        if (orderStatusId != 0) query = query.Where(o => o.order.Orderstatusid == orderStatusId);
        switch (lastDays)
        {
            case "All  Time":
                break;

            case "Last 7 Days":
                query = query.Where(o => o.order.CreatedDate.HasValue &&
                                         o.order.CreatedDate.Value.Date >= DateTime.Now.AddDays(-7).Date);
                break;

            case "Last 30 Days":
                query = query.Where(o => o.order.CreatedDate.HasValue &&
                                         o.order.CreatedDate.Value.Date >= DateTime.Now.AddDays(-30).Date);
                break;
            case "This Month":
                query = query.Where(o => o.order.CreatedDate.HasValue &&
                                         o.order.CreatedDate.Value.Month == DateTime.Now.Month);
                break;
            case "This Year":
                query = query.Where(o => o.order.CreatedDate.HasValue &&
                                       o.order.CreatedDate.Value.Year == DateTime.Now.Year);
                break;

            default:
                break;
        }
        if (startDate != null)
            query = query.Where(o => o.order.CreatedDate.HasValue &&
                                     o.order.CreatedDate.Value.Date >= startDate.Value.Date);
        if (endDate != null)
            query = query.Where(o => o.order.CreatedDate.HasValue &&
                                     o.order.CreatedDate.Value.Date <= endDate.Value.Date);

        var mainquery = query.Select(o => new OrderList
        {
            Orderid = o.order.Orderid,
            CreatedDate = o.order.CreatedDate,
            CustomerName = o.customer.Customername,
            Status = o.orderstatus.Statusname,
            PaymentMode = o.paymentStatus.Paymentmode,
            Rating = o.order.Rating,
            TotalAmount = o.order.Totalamount
        }).OrderBy(o => o.Orderid);

        if (sortCol == "OrderNo")
        {
            if (sortDr == "asc") mainquery = mainquery.OrderBy(x => x.CreatedDate);
            else mainquery = mainquery.OrderByDescending(x => x.CreatedDate);
        }
        else if (sortCol == "OrderDate")
        {
            if (sortDr == "asc") mainquery = mainquery.OrderBy(x => x.CreatedDate);
            else mainquery = mainquery.OrderByDescending(x => x.CreatedDate);
        }
        else if (sortCol == "CustomerName")
        {
            if (sortDr == "asc") mainquery = mainquery.OrderBy(x => x.CustomerName);
            else mainquery = mainquery.OrderByDescending(x => x.CustomerName);
        }
        else if (sortCol == "TotalAmount")
        {
            if (sortDr == "asc") mainquery = mainquery.OrderBy(x => x.TotalAmount);
            else mainquery = mainquery.OrderByDescending(x => x.TotalAmount);
        }
        orders.Count = mainquery.Count();
        if (pageNumber < 1) pageNumber = 1;
        var totalPages = (int)Math.Ceiling((double)orders.Count / pageSize);
        if (pageNumber > totalPages) pageNumber = totalPages;
        if (pageNumber < 1) pageNumber = 1;

        orders.OrderStatusId = orderStatusId;
        orders.Pagenumber = pageNumber;
        orders.Pagesize = pageSize;
        orders.Searchkey = searchKey;
        orders.sortDr = sortDr;
        orders.sortCol = sortCol;
        orders.lastDays = lastDays;
        orders.startDate = startDate;
        orders.endDate = endDate;
        orders.OrderTableLists = mainquery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return orders;
    }

    public List<Orderstatus> GetOrderStatus()
    {
        return (from os in _db.Orderstatuses
                orderby os.Statusname
                select new Orderstatus
                {
                    Orderstatusid = os.Orderstatusid,
                    Statusname = os.Statusname,
                }).ToList();

    }

    public async Task<byte[]> ExportToExcel(int orderStatusId = 0, string lastDays = "All Time", string searchKey = "")
    {
        OrderPage orders = new OrderPage();
        var query = from o in _db.Orders
                    join c in _db.Customers on o.Customerid equals c.Customerid
                    join os in _db.Orderstatuses on o.Orderstatusid equals os.Orderstatusid
                    join ps in _db.Payments on o.Paymentid equals ps.Paymentid
                    where o.Orderid.ToString().ToLower().Contains(searchKey.ToLower()) ||
                          c.Customername.ToLower().Contains(searchKey.ToLower()) ||
                          os.Statusname.ToLower().Contains(searchKey.ToLower()) ||
                          ps.Paymentmode.ToLower().Contains(searchKey.ToLower())
                    orderby o.Orderid
                    select new
                    {
                        order = o,
                        customer = c,
                        orderstatus = os,
                        paymentStatus = ps
                    };


        if (orderStatusId != 0) query = query.Where(o => o.order.Orderstatusid == orderStatusId);
        switch (lastDays)
        {
            case "All  Time":
                break;

            case "Last 7 Days":
                query = query.Where(o => o.order.CreatedDate.HasValue &&
                                         o.order.CreatedDate.Value.Date >= DateTime.Now.AddDays(-7).Date);
                break;

            case "Last 30 Days":
                query = query.Where(o => o.order.CreatedDate.HasValue &&
                                         o.order.CreatedDate.Value.Date >= DateTime.Now.AddDays(-30).Date);
                break;
            case "This Month":
                query = query.Where(o => o.order.CreatedDate.HasValue &&
                                         o.order.CreatedDate.Value.Month == DateTime.Now.Month);
                break;
            case "This Year":
                query = query.Where(o => o.order.CreatedDate.HasValue &&
                                       o.order.CreatedDate.Value.Year == DateTime.Now.Year);
                break;

            default:
                break;
        }
        int totalCount = query.Count();

        List<OrderList> list = await query
                                .Select(o => new OrderList
                                {
                                    Orderid = o.order.Orderid,
                                    CreatedDate = o.order.CreatedDate,
                                    CustomerName = o.customer.Customername,
                                    Status = o.orderstatus.Statusname,
                                    PaymentMode = o.paymentStatus.Paymentmode,
                                    Rating = o.order.Rating ?? 0,
                                    TotalAmount = o.order.Totalamount
                                }).ToListAsync();

        var createExcelHelper = new Helper.CreateExcel(_db);
        return await createExcelHelper.CreateExcelFile(list, searchKey, lastDays, orderStatusId, totalCount);

    }
    

 public async Task<OrderDetailsViewModel> getOrderDetails(int orderId = 1)
    {
        OrderDetailsViewModel model = new OrderDetailsViewModel();

        // basic details 
        model.OrderId = orderId;
        model.OrderStatus = _db.Orderstatuses.Where(o=>o.Orderstatusid == _db.Orders.Where(o=>o.Orderid == orderId).Select(o=>o.Orderstatusid).FirstOrDefault()).Select(o=>o.Statusname).FirstOrDefault();
        model.PaymentStatus = _db.Payments.Where(p=>p.Paymentid == _db.Orders.Where(o=>o.Orderid == orderId).Select(o=>o.Paymentid).FirstOrDefault()).Select(p=>p.Paymentmode).FirstOrDefault();
        model.SubTotal = 0;
        Invoice? invoice = await _db.Invoices.Where(e=>e.Orderid == orderId).FirstOrDefaultAsync();
        if(invoice != null)
        {
            model.InvoiceId = invoice.Invoiceid;
            model.PaidOn = invoice?.Paidon ?? default;
        }

        Order? obj = await _db.Orders.FindAsync(orderId);
        if(obj != null)
        {
            model.CreatedDate = obj.CreatedDate;
            model.CompletedTime = obj?.Completedtime ?? DateTime.MinValue;
            model.Personcount = (short)(obj?.Personcount ?? 0);
        }

        // Customer details 
        Customer? customer = await _db.Customers.FindAsync(obj?.Customerid);
        if(customer != null)
        {
            model.Customer = new CustomerDetails
            {
                Name = customer.Customername,
                Email = customer.Customeremail,
                Phone = customer.PhoneNumber
            };
        }

        // Table details
        List<int?> tableIds = _db.MapOrderTables.Where(o => o.Orderid == orderId).Select(o => (int?)o.Tablesid).ToList();
        model.Tables = new TableDetails();
        model.Tables.TableList = new List<string?>();

        for(int i=0; i<tableIds.Count; i++)
        {
            model.Tables.TableList.Add(_db.Tables.Where(t=>t.Tablesid == tableIds[i]).Select(t=>t.Tablename).FirstOrDefault());
            model.Tables.SectionName = _db.Sections.Where(s=>s.Sectionid == _db.Tables.Where(t=>t.Tablesid == tableIds[i]).Select(t=>t.Sectionid).FirstOrDefault()).Select(s=>s.Sectionname).FirstOrDefault();
        }

        // Dish details
        model.Dishes = (from Dishes in _db.Dishes
                        where Dishes.Orderid == orderId
                        select new DishDetails
                        {
                            Itemname = Dishes.Itemname,
                            Quantity = Dishes.Quantity,
                            Price = Dishes.Price,
                            Total = Dishes.Quantity * Dishes.Price,
                            modifiers = (from dishModifier in _db.Dishmodifiers
                                         where dishModifier.Dishid == Dishes.Dishid
                                         select new ModifierDetails
                                         {
                                            Modifiername = dishModifier.Modifiername,
                                            Quantity = dishModifier.Quantity,
                                            Price = dishModifier.Price,
                                            Total = dishModifier.Quantity *  dishModifier.Price
                                         }).ToList()
                        }).ToList();

        foreach(var dish in model.Dishes)
        {
            model.SubTotal += dish.Total;
            foreach(var modifier in dish.modifiers)
            {
                model.SubTotal += modifier.Total;
            }
        }

        model.Total = model.SubTotal;

        // Taxes details
        model.Taxes = (from InvoiceTax in _db.Invoicetaxes
                        where InvoiceTax.Invoiceid == model.InvoiceId
                        select new TaxDetails
                        {
                            Taxname = InvoiceTax.Taxname,
                            Taxvalue = InvoiceTax.Taxvalue,
                            Taxvaluetype = InvoiceTax.Taxvaluetype,
                            AppliedTax = (InvoiceTax.Taxvaluetype == "Percentage") 
                                          ? Math.Round(model.SubTotal * InvoiceTax.Taxvalue / 100, 2) 
                                          : Convert.ToDecimal(InvoiceTax.Taxvalue)
                        }).ToList();

        foreach(var tax in model.Taxes)
        {
            model.Total += tax.AppliedTax;
        }

        Order objc = await _db.Orders.FindAsync(orderId);
        objc.Totalamount = model.Total;

        _db.SaveChanges();
        
        return model;
    }


}
