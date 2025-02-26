using BusinessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;
using Microsoft.EntityFrameworkCore;

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
                             Userid = user.Userid,
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

    public void AddUser(AddUserViewModel model, string email)
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
            CreatedBy = email

        };

        _db.Users.Add(user);
        _db.SaveChanges();
    }

    public async Task<EditUserViewModel> GetUserProfileDetailsAsync(int userId)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Userid == userId);
        if (user == null) return null;

        return new EditUserViewModel
        {
            Email = user.Email,
            Username = user.Username,
            Phone = user.Phone,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Address = user.Address,
            Zipcode = user.Zipcode,
            Countryid = user.Countryid,
            Stateid = user.Stateid,
            Cityid = user.Cityid,
            Isactive = user.Isactive,
            Roleid = user.Roleid,
            Profilepic = user.Profilepic


        };
    }
    public async Task<bool> EditUserProfileDetailsAsync(EditUserViewModel model)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (user == null) return false;

        user.Firstname = model.Firstname;
        user.Lastname = model.Lastname;
        user.Username = model.Username;
        user.Email = model.Email;
        user.Phone = model.Phone;
        user.Address = model.Address;
        user.Zipcode = model.Zipcode;
        user.Countryid = model.Countryid;
        user.Stateid = model.Stateid;
        user.Cityid = model.Cityid;
        user.Roleid = model.Roleid;
        user.Isactive = model.Isactive;
        user.EditedBy = "Admin";  // Ideally, get from auth
        user.EditDate = DateTime.UtcNow;


        await _db.SaveChangesAsync();
        return true;
    }

}
