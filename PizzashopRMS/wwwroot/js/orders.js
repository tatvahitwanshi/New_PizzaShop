$(document).ready(function () {
    let today = new Date().toISOString().slice(0, 10);
    $("#orderFromDate").attr("max", today);
    $("#orderToDate").attr("max", today);

    $("#orderFromDate").on("change", function () {
        let fromdate = $("#orderFromDate").val();
        $("#orderToDate").attr("min", fromdate);
    });

});

function OrderUpdatePage(Pagenumber = 1, Pagesize = 5, Searchkey = "", OrderStatusId = 0, sortDr = "asc", sortCol = "OrderNo", lastDays = "All Time", startDate = "", endDate = "")
{
    console.log(startDate);
    $.ajax({
        url: "/Orders/OrderListTable",
        type: "GET",
        data: {
            Pagenumber: Pagenumber,
            Pagesize: Pagesize,
            Searchkey: Searchkey,
            sortDr: sortDr,
            sortCol: sortCol,
            OrderStatusId: OrderStatusId,
            lastDays: lastDays,
            startDate: startDate,
            endDate: endDate
        },
        success: function (data) {
            $("#orderTablePartialView").html(data);
        }
    });
}

function UpdatePageSizeOrder()
{
    var params = getDataValues();
    params.Pagenumber = 1;
    params.Pagesize = $("#ordersPerPageList").val();
    OrderUpdatePage(params.Pagenumber, params.Pagesize, params.Searchkey, params.OrderStatusId, params.sortDr, params.sortCol, params.lastDays, params.startDate, params.endDate);
}

function updateListPageTable(dir)
{
    var params = getDataValues();

    if (dir == 'back')
        params.Pagenumber = params.Pagenumber - 1;
    else if (dir == 'next') 
        params.Pagenumber = params.Pagenumber + 1;

    OrderUpdatePage(params.Pagenumber, params.Pagesize, params.Searchkey, params.OrderStatusId, params.sortDr, params.sortCol, params.lastDays, params.startDate, params.endDate);

}

function onKeySearch()
{
    var params = getDataValues();
    params.Pagenumber = 1;
    params.Searchkey = $("#searchOrderInput").val();
    OrderUpdatePage(params.Pagenumber, params.Pagesize, params.Searchkey, params.OrderStatusId, params.sortDr, params.sortCol, params.lastDays, params.startDate, params.endDate);

}

function ApplyFilters()
{
    var params = getDataValues();
    params.Pagenumber = 1;
    params.OrderStatusId = $("#searchOrderSelect").val();
    params.lastDays = $("#searchTimeFilter").val();
    params.sortCol = "Orderid";
    params.sortDr = "asc";
    params.startDate = $("#orderFromDate").val();
    params.endDate = $("#orderToDate").val();
    OrderUpdatePage(params.Pagenumber, params.Pagesize, params.Searchkey, params.OrderStatusId, params.sortDr, params.sortCol, params.lastDays, params.startDate, params.endDate);

}

function ClearFilters()
{
    var params = getDataValues();
    params.Pagenumber = 1;

    params.OrderStatusId = 0;
    $("#searchOrderSelect").val(0);

    params.lastDays = "All Time";
    $("#searchTimeFilter").val("All Time");

    params.sortCol = "OrderNo";
    params.sortDr = "asc";
    
    params.startDate = "";
    $("#orderFromDate").val("");

    params.endDate = "";
    $("#orderToDate").val("");

    params.Searchkey = "";
    $("#searchOrderInput").val("");

    OrderUpdatePage(params.Pagenumber, params.Pagesize, params.Searchkey, params.OrderStatusId, params.sortDr, params.sortCol, params.lastDays, params.startDate, params.endDate);   
}

function sortCol(sortDr, sortCol)
{
    var params = getDataValues();
    params.sortDr = sortDr;
    params.sortCol = sortCol;
    OrderUpdatePage(params.Pagenumber, params.Pagesize, params.Searchkey, params.OrderStatusId, params.sortDr, params.sortCol, params.lastDays, params.startDate, params.endDate);

}