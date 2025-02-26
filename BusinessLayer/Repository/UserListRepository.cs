using BusinessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;

namespace BusinessLayer.Repository;

public class UserListRepository :IUserList
{
    private readonly PizzaShopContext _db;

        public UserListRepository(PizzaShopContext db)
        {
            _db = db;
        }

        public List<UserListViewModel> GetUsers(string sortBy, string sortOrder)
        {
            var userslist = (from user in _db.Users
                             join role in _db.Roles on user.Roleid equals role.Roleid
                             select new UserListViewModel
                             {
                                 Email = user.Email,
                                 Firstname = user.Firstname,
                                 Phone = user.Phone,
                                 RoleName = role.Rolename,
                                 Isactive = user.Isactive
                             }).ToList();

            switch (sortBy)
            {
                case "name":
                    userslist = (sortOrder == "asc") ? userslist.OrderBy(u => u.Firstname).ToList() : userslist.OrderByDescending(u => u.Firstname).ToList();
                    break;
                case "role":
                    userslist = (sortOrder == "asc") ? userslist.OrderBy(u => u.RoleName).ToList() : userslist.OrderByDescending(u => u.RoleName).ToList();
                    break;
                default:
                    userslist = userslist.OrderBy(u => u.Firstname).ToList();
                    break;
            }

            return userslist;
        }

}
