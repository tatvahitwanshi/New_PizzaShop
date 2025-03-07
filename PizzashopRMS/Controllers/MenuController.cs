using BusinessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace PizzashopRMS.Controllers;

public class MenuController : Controller
{
    private readonly IMenu _menu;
    public MenuController(IMenu menu)
    {
        _menu = menu;
    }
    public IActionResult MenuView()
    {
        var model = new MenuViewModel();

        model.Categories = _menu.GetCategories();
        model.Items = _menu.GetItemsByCategory(model.Categories[0].Categoryid); // Pass items for a default category or all items
        model.ItemsUnit = _menu.GetUnits();

        return View(model);
    }
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
    public IActionResult EditCategory(int id)
    {
        var category = _menu.GetCategoryById(id);
        if (category == null)
        {
            return Json(null);
        }
        return Json(new
        {
            categoryid = category.Categoryid,
            categoryname = category.Categoryname,
            categorydescription = category.Categorydescription
        });
    }

    [HttpPost]
    public IActionResult UpdateCategory(Categories category)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var updatedCategory = new Category
                {
                    Categoryid = category.Categoryid,
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
    public IActionResult GetItemsByCategory(int categoryId)
    {
        var model = new MenuViewModel
        {
            Categories = _menu.GetCategories(),
            Items = _menu.GetItemsByCategory(categoryId),
            ItemsUnit = _menu.GetUnits()
        };
        return PartialView("~/Views/Menu/_PartialItems.cshtml", model);
    }

    [HttpGet]
    public IActionResult AddItemsView()
    {
        // var model = new MenuViewModel();
        // model.Categories = _menu.GetCategories();
        // model.ItemsUnit = _menu.GetUnits();
        return PartialView("~/Views/Menu/_PartialItems.cshtml");
    }

    [HttpPost]
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


}
