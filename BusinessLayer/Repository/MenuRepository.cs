using BusinessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;

namespace BusinessLayer.Repository;

public class MenuRepository : IMenu
{
    private readonly PizzaShopContext _db;
    public MenuRepository(PizzaShopContext db)
    {
        _db = db;
    }

    public List<Categories> GetCategories()
    {
        return _db.Categories
            .Where(c => c.Isdeleted != true) 
            .OrderBy(c => c.Categoryname) 
            .Select(c => new Categories
            {
                Categoryid = c.Categoryid,
                Categoryname = c.Categoryname,
                Categorydescription = c.Categorydescription
            })
            .ToList();
    }

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
    public Categories GetCategoryById(int id)
    {
        var category = _db.Categories.FirstOrDefault(c => c.Categoryid == id);
        if (category == null) return null;

        return new Categories
        {
            Categoryid = category.Categoryid,
            Categoryname = category.Categoryname,
            Categorydescription = category.Categorydescription
        };
    }

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



}
