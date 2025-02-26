using BusinessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;

namespace BusinessLayer.Repository;

public class UserListRepository : IUserList
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
    public List<Role> GetRoles()
    {
        return _db.Roles.ToList();
    }

    public List<Country> GetCountries()
    {
        return _db.Countries.ToList();
    }

    public List<State> GetStatesByCountry(int countryId)
    {
        return _db.States.Where(s => s.Countryid == countryId).ToList();
    }

    public List<City> GetCitiesByState(int stateId)
    {
        return _db.Cities.Where(c => c.Stateid == stateId).ToList();
    }

    public void AddUser(AddUserViewModel model , string email)
    {
        var user = new User
        {
            Email = model.Email,
            Username = model.Username,
            Password = model.Password, // Ideally, hash the password before saving
            Firstname = model.Firstname,
            Lastname = model.Lastname,
            Profilepic = model.Profilepic,
            Zipcode = model.Zipcode,
            Address = model.Address,
            Phone = model.Phone,
            Countryid = model.Countryid,
            Stateid = model.Stateid,
            Cityid = model.Cityid,
            Roleid = model.Roleid,
            Isactive = true, // Defaulting new users as active
            CreatedBy=email

        };

        _db.Users.Add(user);
        _db.SaveChanges();
    }

}
