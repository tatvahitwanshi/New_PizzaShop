
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;

namespace DataAccessLayer.ViewModels;

public class MenuViewModel
{
    public List<Categories> Categories { get; set; } = new List<Categories>();

    public Pagination<ItemsView> Items { get; set; } = new Pagination<ItemsView>();

    public List<ItemsUnit> ItemsUnit { get; set; } = new List<ItemsUnit>();

    public List<AddItemsViewModel> AddItemsList { get; set; } = new List<AddItemsViewModel>();

    public List<ModifierGroupModel> ModifierGroupModel { get; set; } = new List<ModifierGroupModel>();

    public Pagination<ModifierItemViewModel> ModifierItemViewModel { get; set; } = new Pagination<ModifierItemViewModel>();

    public Pagination<ModifierItemViewModel> ModifierItemAll { get; set; } = new Pagination<ModifierItemViewModel>();

    public AddEditModifierItemViewModel AddEditModItem {get; set;}

}
public class Categories
{
    public int CategoryId { get; set; }

    public string Categoryname { get; set; } = null!;

    public string? Categorydescription { get; set; }

    public bool? Isdeleted { get; set; }

}

public class ItemsView
{
    public int ItemId { get; set; }

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

    public IFormFile? Itemimage { get; set; }

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

    public  List<AddModGroupWithItem> AddModGroupWithItems {get; set;}

}

public class AddModGroupWithItem{
    public int ModifierGroupId{get; set;}
    public int min{get; set;}
    public int max{get; set;}
 
}

public class ModifierGroupDetails{
    public int ModifierGroupId{get; set;}
    public string ModifierGroupName{get; set;}
    public int max {get; set;}
    public int min {get; set;}
    public  List<ItemShow> ItemShows {get; set;}
    
}
public class ItemShow{
    public required string ModifierItemName{get; set;}
    public int Rate {get; set;}
}

public class ModifierGroupModel
{
    public int ModifierGroupId { get; set; }

    public string ModifierGroupName { get; set; } = null!;

    public string? ModifierGroupDescription { get; set; }

    public bool? Isdeleted { get; set; }
}

public class ModifierItemViewModel
{
    public int ModifierItemId { get; set; }

    public int ModifierGroupId { get; set; }

    public string ModifierItemName { get; set; } = null!;

    public int Rate { get; set; }

    public int Quantity { get; set; }

    public string? ModifierItemDescription { get; set; }

    // public string? EditedBy { get; set; }

    // public string? CreatedBy { get; set; }

    // public DateTime? EditDate { get; set; }

    // public DateTime? CreatedDate { get; set; }

    public int? Modifiersunit { get; set; }

    public string? ModifierUnitname { get; set; }


}

public class AddEditModifierItemViewModel
{
    public int ModifierItemId { get; set; }
    public int ModifierGroupId { get; set; }

    public List<int> ModifierGroupIds { get; set; } = new List<int>(); // Change from int to List<int>

    public string ModifierItemName { get; set; } = null!;

    public int Rate { get; set; }

    public int Quantity { get; set; }

    public string? ModifierItemDescription { get; set; }

    public string? EditedBy { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? EditDate { get; set; }

    public int? Modifiersunit { get; set; }

    public string? ModifierUnitname { get; set; }


}
