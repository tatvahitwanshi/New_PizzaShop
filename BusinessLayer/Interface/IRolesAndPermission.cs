using DataAccessLayer.ViewModels;

namespace BusinessLayer.Interface;

public interface IRolesAndPermission
{
    Task<List<RoleViewModel>> GetRolesAsync();

}
