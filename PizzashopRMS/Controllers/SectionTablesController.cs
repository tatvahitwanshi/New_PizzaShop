using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BusinessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using PizzashopRMS.Helpers;

namespace PizzashopRMS.Controllers;

[CustomAuthorise(new string[] { "admin" })]
public class SectionTablesController : Controller
{
    private readonly ISectionTables _sectionTables;

    // Constructor to initialize menu service
    public SectionTablesController(ISectionTables sectionTables)
    {
        _sectionTables = sectionTables;
    }

    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult SectionTablesView(int sectionId = -1)
    {
        SectionTablesViewModal modal = new SectionTablesViewModal();
        modal.SectionViewModals = _sectionTables.GetSections();
        modal.AddEditSection = new AddEditSectionViewModal();
        // modal.TablesViews=_sectionTables.GetTableBySection();
        if (sectionId == -1) modal.TablesViews = _sectionTables.GetTableBySection(modal.SectionViewModals[0].SectionId);
        else modal.TablesViews = _sectionTables.GetTableBySection(sectionId);


        return View(modal);
    }

    [HttpPost]
    public IActionResult AddSections(SectionTablesViewModal model)
    {
        if (model.AddEditSection != null &&
            !string.IsNullOrEmpty(model.AddEditSection.Sectionname) &&
            !string.IsNullOrEmpty(model.AddEditSection.Sectiondescription))
        {
            var email = _sectionTables.GetEmailFromToken(Request);

            if (email == null)
            {
                TempData["error"] = "User not authenticated!";
                return RedirectToAction("SectionTablesView");
            }

            var newSection = new Section
            {
                Sectionname = model.AddEditSection.Sectionname,
                Sectiondescription = model.AddEditSection.Sectiondescription
            };

            TempData["success"] = "Section added successfully!";
            _sectionTables.AddSections(newSection, email);

            return RedirectToAction("SectionTablesView");
        }

        TempData["error"] = "Invalid data provided!";
        return View("SectionTablesView");
    }

    // Fetches Section details for editing
    public IActionResult EditSection(int id)
    {
        var sections = _sectionTables.GetSectionById(id);
        if (sections == null)
        {
            return Json(null);
        }
        return Json(new
        {
            sectionid = sections.SectionId,
            sectionname = sections.Sectionname,
            sectiondescription = sections.Sectiondescription
        });
    }

    // Updates an existing category
    [HttpPost]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult UpdateSection(SectionTablesViewModal section)
    {
        AddEditSectionViewModal sec = section.AddEditSection;

        if (sec != null)
        {
            var email = _sectionTables.GetEmailFromToken(Request);
            try
            {
                var updatedSection = new Section
                {
                    Sectionid = sec.SectionId,
                    Sectionname = sec.Sectionname,
                    Sectiondescription = sec.Sectiondescription
                };

                _sectionTables.UpdateSection(updatedSection, email);
                TempData["success"] = "Section updated successfully!";
            }
            catch (Exception)
            {
                TempData["error"] = "An error occurred while updating the section.";
            }
        }
        else
        {
            TempData["error"] = "Invalid data for section. Please check the inputs.";
        }

        return RedirectToAction("SectionTablesView");
    }

    // Soft deletes a category
    [HttpPost]
    public IActionResult DeleteSection(int sectionId)
    {
        bool isDeleted = _sectionTables.SoftDeleteSection(sectionId);
        if (isDeleted)
        {
            TempData["success"] = "Section deleted successfully!";
            return Json(new { success = true });
        }
        else
        {
            TempData["error"] = "Failed to delete Section!";
            return Json(new { success = false });
        }
    }

    public IActionResult GetTablesBySection(int sectionId, int PageNumber = 1, int PageSize = 5, string SearchKey = "")
    {
        var modal = new SectionTablesViewModal
        {
            TablesViews = _sectionTables.GetTableBySection(sectionId, PageNumber, PageSize, SearchKey),
            SectionViewModals = _sectionTables.GetSections()
        };
        return PartialView("~/Views/SectionTables/_PartialTable.cshtml", modal);
    }

    [HttpPost]
    public IActionResult AddTable(SectionTablesViewModal model)
    {
        AddEditTablesView tablemodel = model.AddEditTables;

        if (tablemodel != null)
        {
            // Log the SectionId for debugging
            Console.WriteLine($"SectionId: {tablemodel.SectionId}");

            if (tablemodel.SectionId == null || tablemodel.SectionId == 0)
            {
                TempData["error"] = "SectionId is null or invalid!";
                return RedirectToAction("SectionTablesView");
            }

            var email = _sectionTables.GetEmailFromToken(Request);
            var result = _sectionTables.AddTable(tablemodel, email);

            if (result)
            {
                TempData["success"] = "Table added successfully!";
                return RedirectToAction("SectionTablesView");
            }
            else
            {
                TempData["error"] = "Failed to add table. Please try again.";
                return RedirectToAction("SectionTablesView");
            }
        }

        TempData["error"] = "Validation failed. Please check the input fields.";
        model.SectionViewModals = _sectionTables.GetSections();
        model.TablesViews = _sectionTables.GetTableBySection(model.AddEditTables.SectionId ?? -1);
        return View("SectionTablesView", model);
    }

    public async Task<IActionResult> GetTableDetails(int tableId)
    {
        var tableDetails = await _sectionTables.GetTableByIdAsync(tableId);
        if (tableDetails == null)
        {
            return NotFound();
        }

        return Json(tableDetails);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateTable(SectionTablesViewModal model)
    {
        AddEditTablesView tablemodel=model.AddEditTables;
        if (tablemodel != null)
        {
            var isUpdated = _sectionTables.UpdateTable(tablemodel);
            if (isUpdated)
            {
                TempData["success"] = "Table updated successfully.";
                return RedirectToAction("SectionTablesView"); // Redirect back to your main view.
            }
            else
            {
                ModelState.AddModelError("", "Unable to update the table. Please try again.");
            }
        }

        // If the update fails, you can redirect back to the `SectionTablesView` page with validation errors.
        TempData["error"] = "Validation failed. Please review the form.";
        return RedirectToAction("SectionTablesView");
    }


}
