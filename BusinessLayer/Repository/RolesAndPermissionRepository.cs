using BusinessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Repository;

public class RolesAndPermissionRepository : IRolesAndPermission
{
    private readonly PizzaShopContext _db;

    public RolesAndPermissionRepository(PizzaShopContext db)
    {
        _db = db;
       
    }
     public async Task<List<RoleViewModel>> GetRolesAsync()
        {
            return await _db.Roles
                .Select(r => new RoleViewModel { Rolename = r.Rolename })
                .ToListAsync();
        }
}
