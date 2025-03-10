using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;

namespace BusinessLayer.Interface;

public interface IMenu
{
    List<Categories> GetCategories();
    void AddCategory(Category category);
    Categories GetCategoryById(int id); // Get category by ID
    void UpdateCategory(Category category); // Update category
    bool SoftDeleteCategory(int categoryId);
    List<ItemsView> GetItemsByCategory(int categoryId);
    List<ItemsUnit> GetUnits();
    Task<string> AddItems(AddItemsViewModel model);
    AddItemsViewModel GetItemById(int id);
    Task<bool> UpdateItem(AddItemsViewModel item);


}
