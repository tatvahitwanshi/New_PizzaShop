using BusinessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;

namespace BusinessLayer.Repository;

public class MenuRepository : IMenu
{
    private readonly PizzaShopContext _db;
    private readonly IUserList _userListRepository;

    // Constructor to initialize database context and user repository
    public MenuRepository(PizzaShopContext db, IUserList userListRepository)
    {
        _db = db;
        _userListRepository = userListRepository;
    }

    // Retrieves a list of active categories sorted by name
    public List<Categories> GetCategories()
    {
        return _db.Categories
            .Where(c => c.Isdeleted != true)
            .OrderBy(c => c.Categoryname)
            .Select(c => new Categories
            {
                CategoryId = c.Categoryid,
                Categoryname = c.Categoryname,
                Categorydescription = c.Categorydescription
            })
            .ToList();
    }

    // Retrieves all available item units sorted by name
    public List<ItemsUnit> GetUnits()
    {
        return _db.ItemsUnits
            .OrderBy(i => i.Unitname)
            .Select(i => new ItemsUnit
            {
                Unitid = i.Unitid,
                Unitname = i.Unitname,

            })
          .ToList();

    }

    // Adds a new category to the database
    public void AddCategory(Category category)
    {
        var newCategory = new Category
        {
            Categoryname = category.Categoryname,
            Categorydescription = category.Categorydescription
        };

        _db.Categories.Add(newCategory);
        _db.SaveChanges();
    }

    // Retrieves a specific category by its ID
    public Categories GetCategoryById(int id)
    {
        var category = _db.Categories.FirstOrDefault(c => c.Categoryid == id);
        if (category == null) throw new KeyNotFoundException($"Category with ID {id} not found.");

        return new Categories
        {
            CategoryId = category.Categoryid,
            Categoryname = category.Categoryname,
            Categorydescription = category.Categorydescription
        };
    }

    // Updates an existing category in the database
    public void UpdateCategory(Category category)
    {
        var existingCategory = _db.Categories.FirstOrDefault(c => c.Categoryid == category.Categoryid);
        if (existingCategory != null)
        {
            existingCategory.Categoryname = category.Categoryname;
            existingCategory.Categorydescription = category.Categorydescription;
            _db.SaveChanges();
        }
    }

    // Performs a soft delete on a category by marking it as deleted
    public bool SoftDeleteCategory(int categoryId)
    {
        var category = _db.Categories.FirstOrDefault(c => c.Categoryid == categoryId);
        if (category != null)
        {
            category.Isdeleted = true;
            _db.SaveChanges();
            return true;
        }
        return false;
    }

    // Retrieves all items in a specific category
    public List<ItemsView> GetItemsByCategory(int categoryId)
    {
        return _db.Items
            .Where(i => i.Categoryid == categoryId && i.Isdeleted != true)
            .Select(i => new ItemsView
            {
                ItemId = i.Itemid,
                Itemname = i.Itemname,
                Rate = i.Rate,
                Itemtype = i.Itemtype,
                Quantity = i.Quantity,
                Isavailable = i.Isavailable,
                Itemdescription = i.Itemdescription,
                Itemimage = i.Itemimage,
                Categoryid = i.Categoryid
            }).ToList();
    }

    // Adds a new item to the menu if it does not already exist
    public async Task<string> AddItems(AddItemsViewModel model)
    {
        Item Uniqueitem = _db.Items.FirstOrDefault(i => i.Itemname == model.Itemname);
        if (Uniqueitem != null)
        {
            return "Item already exists!";
        }
        var item = new Item
        {
            Itemname = model.Itemname,
            Rate = model.Rate,
            Itemtype = model.Itemtype,
            Quantity = model.Quantity,
            Isavailable = model.Isavailable,
            Itemdescription = model.Itemdescription,
            Itemimage = await _userListRepository.UploadPhotoAsync(model.Itemimage),
            Categoryid = model.CategoryId,
            Itemid = model.ItemId,
            Unitid = model.UnitId,
            Defaulttax = model.Defaulttax,
            Taxpercentage = model.Taxpercentage,
            Shortcode = model.Shortcode

        };
        _db.Add(item);
        await _db.SaveChangesAsync();
        return "Item added successfully!";
    }

    public AddItemsViewModel GetItemById(int id)
    {
        var item = _db.Items
            .Where(i => i.Itemid == id)
            .Select(i => new AddItemsViewModel
            {
                ItemId = i.Itemid,
                CategoryId = i.Categoryid,
                Itemname = i.Itemname,
                Itemtype = i.Itemtype,
                Rate = (int)i.Rate,
                Quantity = (int)i.Quantity,
                UnitId = i.Unitid,
                Isavailable = (bool)i.Isavailable,
                Defaulttax = i.Defaulttax,
                Taxpercentage = i.Taxpercentage,
                Shortcode = i.Shortcode,
                Itemdescription = i.Itemdescription
            })
            .FirstOrDefault();

        return item;
    }
    public async Task<bool> UpdateItem(AddItemsViewModel item)
    {
        var existingItem = await _db.Items.FindAsync(item.ItemId);
        if (existingItem == null)
            return false;

        // Update item properties
        existingItem.Categoryid = item.CategoryId;
        existingItem.Itemname = item.Itemname;
        existingItem.Itemtype = item.Itemtype;
        existingItem.Rate = item.Rate;
        existingItem.Quantity = item.Quantity;
        existingItem.Unitid = item.UnitId;
        existingItem.Isavailable = item.Isavailable;
        existingItem.Defaulttax = item.Defaulttax;
        existingItem.Taxpercentage = item.Taxpercentage;
        existingItem.Shortcode = item.Shortcode;
        existingItem.Itemdescription = item.Itemdescription;
        existingItem.Itemimage = await _userListRepository.UploadPhotoAsync(item.Itemimage);
        _db.Items.Update(existingItem);
        await _db.SaveChangesAsync();
        return true;
    }

}
