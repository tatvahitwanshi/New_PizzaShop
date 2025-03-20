using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.ViewModels;

public class SectionTablesViewModal
{
    public List<SectionViewModal> SectionViewModals { get; set; } = new List<SectionViewModal>();

    public AddEditSectionViewModal AddEditSection {get; set;}
}
public class SectionViewModal
{
    public int Sectionid { get; set; }
    public string? Sectionname { get; set; }
    public string? Sectiondescription { get; set; }
    public bool? Isdeleted { get; set; }

}

public class AddEditSectionViewModal
{
    public int Sectionid { get; set; }
    
    [Required(ErrorMessage = "Section name is required.")]
    public string? Sectionname { get; set; }

    [Required(ErrorMessage = "Section description is required.")]
    public string? Sectiondescription { get; set; }
    public bool? Isdeleted { get; set; }
    public string? CreatedBy { get; set; }
    public string? EditedBy { get; set; }
    public DateTime? EditDate { get; set; }

}
