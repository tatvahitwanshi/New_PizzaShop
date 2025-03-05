
namespace DataAccessLayer.ViewModels;

public class MenuViewModel
{
    public List<Categories> Categories { get; set; } = new List<Categories>();
    public List<Items> Items { get; set; } = new List<Items>();


}

public class Categories
{
    public int Categoryid { get; set; }

    public string Categoryname { get; set; } = null!;

    public string? Categorydescription { get; set; }
    public bool? Isdeleted { get; set; }

}

public class Items
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


    public int Unitid { get; set; }

    public string? CreatedBy { get; set; }

    public string? EditedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? EditDate { get; set; }

}
