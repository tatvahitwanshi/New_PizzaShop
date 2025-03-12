using BusinessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using PizzashopRMS.Helpers;

namespace PizzashopRMS.Controllers;

[CustomAuthorise(new string[] { "admin" })]
public class MenuController : Controller
{
    private readonly IMenu _menu;

    // Constructor to initialize menu service
    public MenuController(IMenu menu)
    {
        _menu = menu;
    }

    // Displays the menu view with categories, items, and units
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult MenuView()
    {
        var model = new MenuViewModel();

        model.Categories = _menu.GetCategories();
        model.Items = _menu.GetItemsByCategory(model.Categories[0].CategoryId); // Pass items for a default category or all items
        model.ItemsUnit = _menu.GetUnits();
        model.ModifierGroupModel = _menu.GetModifierGroups();
        model.ModifierItemViewModel = _menu.GetModifierItemsByModifierGroup(model.ModifierGroupModel[0].ModifierGroupId); // Pass items for a default category or all items
        model.ModifierItemAll=_menu.GetAllModifierItems();
        return View(model);
    }

    // Adds a new category
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpPost]
    public IActionResult AddCategory(Category category)
    {
        if (category.Categorydescription != null && category.Categoryname != null)
        {
            var newCategory = new Category
            {
                Categoryname = category.Categoryname,
                Categorydescription = category.Categorydescription
            };
            TempData["success"] = "Category added successfully!";
            _menu.AddCategory(newCategory);
            return RedirectToAction("MenuView");
        }
        return View("MenuView");
    }

    // Fetches category details for editing
    public IActionResult EditCategory(int id)
    {
        var category = _menu.GetCategoryById(id);
        if (category == null)
        {
            return Json(null);
        }
        return Json(new
        {
            categoryid = category.CategoryId,
            categoryname = category.Categoryname,
            categorydescription = category.Categorydescription
        });
    }

    // Updates an existing category
    [HttpPost]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult UpdateCategory(Categories category)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var updatedCategory = new Category
                {
                    Categoryid = category.CategoryId,
                    Categoryname = category.Categoryname,
                    Categorydescription = category.Categorydescription
                };

                _menu.UpdateCategory(updatedCategory);
                TempData["success"] = "Category updated successfully!";
            }
            catch (Exception)
            {
                TempData["error"] = "An error occurred while updating the category.";
            }
        }
        else
        {
            TempData["error"] = "Invalid data. Please check the inputs.";
        }

        return RedirectToAction("MenuView");
    }

    // Soft deletes a category
    [HttpPost]
    public IActionResult DeleteCategory(int categoryId)
    {
        bool isDeleted = _menu.SoftDeleteCategory(categoryId);
        if (isDeleted)
        {
            TempData["success"] = "Category deleted successfully!";
            return Json(new { success = true });
        }
        else
        {
            TempData["error"] = "Failed to delete category!";
            return Json(new { success = false });
        }
    }

    // Retrieves items based on selected category
    public IActionResult GetItemsByCategory(int categoryId ,int PageNumber=1, int PageSize=5, string SearchKey="")
    {
        var model = new MenuViewModel
        {
            Categories = _menu.GetCategories(),
            Items = _menu.GetItemsByCategory(categoryId, PageNumber, PageSize, SearchKey),
            ItemsUnit = _menu.GetUnits()
        };
        return PartialView("~/Views/Menu/_PartialItems.cshtml", model);
    }
   

    // Displays the view for adding new items
    [HttpGet]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult AddItemsView()
    {
        return PartialView("~/Views/Menu/_PartialItems.cshtml");
    }

    // Adds a new item 
    [HttpPost]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<JsonResult> AddItemsPost(AddItemsViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data. Please check the inputs." });
            }

            var success = await _menu.AddItems(model);
            TempData["success"] = "Item added successfully!";
            return Json(new { success = true, redirectUrl = "/Menu/MenuView" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "An error occurred while adding the item." });
        }
    }

    // Fetches item details for editing
    [HttpGet]
    public IActionResult GetItemById(int id)
    {
        var item = _menu.GetItemById(id);
        if (item == null)
        {
            return NotFound();
        }
        return Json(item);
    }

    // Updates an existing item
    [HttpPost]
    public async Task<IActionResult> EditItems(AddItemsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Invalid input data" });
        }

        bool isUpdated = await _menu.UpdateItem(model);

        if (isUpdated)
        {
            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = false, message = "Item not found or could not be updated." });
        }
    }

    // Soft deletes a category
    [HttpPost]
    public IActionResult DeleteItem(List<int> itemIds)
    {
        bool isDeleted = _menu.SoftDeleteItems(itemIds);
        if (isDeleted)
        {
            TempData["success"] = "Items deleted successfully!";
            return Json(new { success = true });
        }
        else
        {
            TempData["error"] = "Failed to delete items!";
            return Json(new { success = false });
        }
    }

    // Adds a new modifier group
    [HttpPost]
    public IActionResult AddModifierGroup(Modifiersgroup modifier)
    {
        if (modifier.Modifiersgroupdescription != null && modifier.Modifiersgroupname != null)
        {
            var newModifier = new Modifiersgroup
            {

                Modifiersgroupname = modifier.Modifiersgroupname,
                Modifiersgroupdescription = modifier.Modifiersgroupdescription
            };

            TempData["success"] = "Modifier added successfully!";
            _menu.AddModifier(newModifier);
            return RedirectToAction("MenuView");
        }
        return View("MenuView");
    }

    // Edit modifier group
    [HttpGet]
    public IActionResult EditModifierGroup(int id)
    {
        var modifier = _menu.GetModifierById(id);
        if (modifier == null)
        {
            return Json(null);
        }
        return Json(new
        {
            modifiergroupid = modifier.ModifierGroupId,
            modifiergroupname = modifier.ModifierGroupName,
            modifiergroupdescription = modifier.ModifierGroupDescription
        });
    }

    // Updates an existing category
    [HttpPost]
    public IActionResult UpdateModififerGroup(ModifierGroupModel modifier)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var updatedModifier = new Modifiersgroup
                {
                    Modifiersgroupid = modifier.ModifierGroupId,
                    Modifiersgroupname = modifier.ModifierGroupName,
                    Modifiersgroupdescription = modifier.ModifierGroupDescription
                };


                _menu.UpdateModifier(updatedModifier);
                TempData["success"] = "Modifier Group updated successfully!";
            }
            catch (Exception)
            {
                TempData["error"] = "An error occurred while updating the Modifier Group.";
            }
        }
        else
        {
            TempData["error"] = "Invalid data. Please check the inputs.";
        }

        return RedirectToAction("MenuView");
    }

    // Soft deletes a category
    [HttpPost]
    public IActionResult DeleteModfierGroup(int modifiergroupid)
    {
        bool isDeleted = _menu.SoftDeleteModfierGroup(modifiergroupid);
        if (isDeleted)
        {
            TempData["success"] = "Category deleted successfully!";
            return Json(new { success = true });
        }
        else
        {
            TempData["error"] = "Failed to delete category!";
            return Json(new { success = false });
        }
    }

    // Retrieves items based on selected category
    public IActionResult GetModifierItemsByModifierGroup(int modifiergroupid , int PageNumber=1, int PageSize=5, string SearchKey="")
    {
        var model = new MenuViewModel
        {
            ModifierGroupModel =_menu.GetModifierGroups(),
            ModifierItemViewModel = _menu.GetModifierItemsByModifierGroup(modifiergroupid , PageNumber, PageSize, SearchKey),
            ItemsUnit = _menu.GetUnits()
        };
        return PartialView("~/Views/Menu/_PartialModifier.cshtml", model);
    }

}
