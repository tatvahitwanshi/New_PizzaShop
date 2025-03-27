using BusinessLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using PizzashopRMS.Helpers;

namespace PizzashopRMS.Controllers;

[CustomAuthorise(new string[] { "admin" })]
public class TaxesFeesController : Controller
{
    private readonly ITaxesFees _taxes;
    public TaxesFeesController(ITaxesFees taxes)
    {
        _taxes = taxes;
    }

    public IActionResult TaxesFeesView()
    {
        // Fetch data using the repository
        var taxesFeesViewModel = new TaxesFeesViewModel
        {
            TaxesViewModal = _taxes.GetAllTaxesFees()
        };

        return View(taxesFeesViewModel);
    }
    public IActionResult TaxesFeesTableView(string SearchKey="")
    {
        // Fetch data using the repository
        var taxesFeesViewModel = new TaxesFeesViewModel
        {
            TaxesViewModal = _taxes.GetAllTaxesFees(SearchKey)
        };

        return PartialView("_PartialTaxesFees", taxesFeesViewModel);
    }

    [HttpPost]
    public IActionResult AddTax(TaxesFeesViewModel model)
    {
        if (ModelState.IsValid)
        {
            var addTaxe = model.AddEditTaxe;
            var email = _taxes.GetEmailFromToken(Request);
            var result = _taxes.AddTax(addTaxe, email);
            if (result)
            {
                TempData["success"] = "Tax added successfully!";
            }
            else
            {
                TempData["error"] = "Failed to add tax.";
            }
        }
        else
        {
            TempData["error"] = "Please correct the errors in the form.";
        }

        return RedirectToAction("TaxesFeesView");
    }

    [HttpGet]
    public IActionResult GetTaxDetails(int id)
    {
        var taxDetails = _taxes.GetTaxById(id);
        if (taxDetails == null)
        {
            return NotFound("Tax details not found");
        }
        return PartialView("_PartialEditModal", taxDetails);
    }

    [HttpPost]
    public IActionResult EditTax(TaxesFeesViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("_PartialEditModal", model);
        }

        bool success = _taxes.UpdateTax(model);
        if (success)
        {
            TempData["success"] = "Tax updated successfully!";
        }
        else
        {
            TempData["error"] = "Error occur while updation tax";
        }

        return RedirectToAction("TaxesFeesView");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTaxes(int taxId)
    {
        try
        {
            await _taxes.DeleteTax(taxId);

            // Re-render the table partial view after deletion
            var taxesFeesViewModel = new TaxesFeesViewModel
            {
                TaxesViewModal = _taxes.GetAllTaxesFees()
            };

            return PartialView("_PartialTaxesFees", taxesFeesViewModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "An error occurred while deleting the tax.");
        }
    }

}
