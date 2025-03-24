using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Interface;

public interface ISectionTables
{
    string? GetEmailFromToken(HttpRequest request);

    List<SectionViewModal> GetSections();
    void AddSections(Section sections,string email);
    AddEditSectionViewModal GetSectionById(int id); // Get category by ID
    void UpdateSection(Section sections,string email);
    bool SoftDeleteSection(int sectionId);
    Pagination<TablesView> GetTableBySection(int sectionId, int PageNumber = 1, int PageSize = 3, string SearchKey = "");
    bool AddTable(AddEditTablesView model, string email);
    Task<AddEditTablesView> GetTableByIdAsync(int tableId); // Fetch table details by ID
    bool UpdateTable(AddEditTablesView table);

    bool SoftDeleteTable(List<int> tableIds);

}
