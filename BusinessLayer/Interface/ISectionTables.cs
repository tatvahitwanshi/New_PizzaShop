using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;

namespace BusinessLayer.Interface;

public interface ISectionTables
{
    List<SectionViewModal> GetSections();
    void AddSections(Section sections);
    AddEditSectionViewModal GetSectionById(int id); // Get category by ID



}
