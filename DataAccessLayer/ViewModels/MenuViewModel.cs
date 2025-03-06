
namespace DataAccessLayer.ViewModels;

public class MenuViewModel
{
    public List<Categories> Categories { get; set; } = new List<Categories>();
    public List<ItemsView> Items { get; set; } = new List<ItemsView>();


}

public class Categories
{
    public int Categoryid { get; set; }
    public string Categoryname { get; set; } = null!;
    public string? Categorydescription { get; set; }
    public bool? Isdeleted { get; set; }

}

public class ItemsView
{
    public int Itemid { get; set; }

    public string Itemname { get; set; } = null!;

    public int? Rate { get; set; }

    public string Itemtype { get; set; } = null!;

    public int? Quantity { get; set; }

    public bool? Isavailable { get; set; }

    public bool? Isdeleted { get; set; }

    public string? Itemdescription { get; set; }

    public string? Itemimage { get; set; }

    public int Categoryid { get; set; }


}

public class AddItemsViewModel
{
    public int CategoryId { get; set; }

    public int ItemId { get; set; }

    public string Itemname { get; set; } = null!;

    public int Rate { get; set; }

    public string Itemtype { get; set; } = null!;

    public int Quantity { get; set; }

    public bool Isavailable { get; set; }

    public bool Isdeleted { get; set; }

    public string? Itemdescription { get; set; }

    public string? Itemimage { get; set; }

    public int UnitId { get; set; }

    public string? EditedBy { get; set; }

    public string? CreatedBy { get; set; }
    public DateTime? EditDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    public bool? Defaulttax { get; set; }
    public decimal? Taxpercentage { get; set; }
    public string? Shortcode { get; set; }
     public List<Categories> Categories { get; set; } = new List<Categories>();
    public List<ItemsView> Items { get; set; } = new List<ItemsView>();

}
