namespace DataAccessLayer.ViewModels;

public class PermissionUpdateRequest
{
     public int RoleId { get; set; }
    public List<PermissionViewModel> Permissions { get; set; }
}
