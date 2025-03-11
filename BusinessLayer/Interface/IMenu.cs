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
    Pagination<ItemsView> GetItemsByCategory(int categoryId , int PageNumber=1, int PageSize=3, string SearchKey="");
    List<ItemsUnit> GetUnits();
    Task<string> AddItems(AddItemsViewModel model);
    AddItemsViewModel GetItemById(int id);
    Task<bool> UpdateItem(AddItemsViewModel item);

    bool SoftDeleteItems(List<int> itemIds);

    //---------- Modifier Group------------
    List<ModifierGroupModel> GetModifierGroups();
    void AddModifier(Modifiersgroup modifiersgroup);
    ModifierGroupModel GetModifierById(int id); 
    void UpdateModifier(Modifiersgroup modifiersgroup); 
    bool SoftDeleteModfierGroup (int modifiergroupid);
    List<ModifierItemViewModel> GetModifierItemsByModifierGroup(int modifiergroupid);


}
