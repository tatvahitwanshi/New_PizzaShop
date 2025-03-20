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
    public IActionResult SectionTablesView()
    {
        var modal = new SectionTablesViewModal();
        modal.SectionViewModals = _sectionTables.GetSections();
        modal.AddEditSection = new AddEditSectionViewModal();

        return View(modal);
    }

    [HttpPost]
    public IActionResult AddSections(SectionTablesViewModal model)
    {
        if (model.AddEditSection != null && !string.IsNullOrEmpty(model.AddEditSection.Sectionname) && !string.IsNullOrEmpty(model.AddEditSection.Sectiondescription))
        {
            var newSection = new Section
            {
                Sectionname = model.AddEditSection.Sectionname,
                Sectiondescription = model.AddEditSection.Sectiondescription
            };
            TempData["success"] = "Section added successfully!";
            _sectionTables.AddSections(newSection);
            return RedirectToAction("SectionTablesView");
        }
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
            sectionid = sections.Sectionid,
            sectionname = sections.Sectionname,
            sectiondescription = sections.Sectiondescription
        });
    }


}
