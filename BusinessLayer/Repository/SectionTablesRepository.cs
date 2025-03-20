using BusinessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;

namespace BusinessLayer.Repository;

public class SectionTablesRepository : ISectionTables
{
    private readonly PizzaShopContext _db;
    
    public SectionTablesRepository(PizzaShopContext db)
    {
        _db=db;
    }
    public List<SectionViewModal> GetSections()
    {
        return _db.Sections
            .Where(c=>c.Isdeleted != true)
            .OrderBy(c=>c.Sectionid)
            .Select(c=>new SectionViewModal
            {
                Sectionid=c.Sectionid,
                Sectionname=c.Sectionname,
                Sectiondescription=c.Sectiondescription
            }).ToList();
    }

    public void AddSections(Section sections)
    {
        var newSection=new Section
        {
            Sectionname=sections.Sectionname,
            Sectiondescription=sections.Sectiondescription
        };
        _db.Sections.Add(newSection);
        _db.SaveChanges();
    }
    
    // Retrieves a specific category by its ID
    public AddEditSectionViewModal GetSectionById(int id)
    {
        var sections = _db.Sections.FirstOrDefault(c => c.Sectionid == id);
        if (sections == null) throw new KeyNotFoundException($"Section with ID {id} not found.");

        return new AddEditSectionViewModal
        {
            Sectionid = sections.Sectionid,
            Sectionname = sections.Sectionname,
            Sectiondescription = sections.Sectiondescription
        };
    }

}
