using BusinessLayer.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;
using Microsoft.EntityFrameworkCore;
using PizzaShopApp.Helpers;
using BusinessLayer.Helper;

namespace BusinessLayer.Repository;

public class UserProfileRepository : IUserProfile
{
    private readonly PizzaShopContext _db;

        public UserProfileRepository(PizzaShopContext db)
        {
            _db = db;
        }

        public async Task<UserProfileViewModel> GetUserProfileAsync(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            return new UserProfileViewModel
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
                CountryList = await _db.Countries.ToListAsync(),
                StateList = await _db.States.ToListAsync(),
                CityList = await _db.Cities.ToListAsync()
            };
        }

        public async Task<bool> UpdateUserProfileAsync(UserProfileViewModel model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null) return false;

            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;
            user.Username = model.Username;
            user.Phone = model.Phone;
            user.Address = model.Address;
            user.Zipcode = model.Zipcode;
            user.Countryid = model.Countryid;
            user.Stateid = model.Stateid;
            user.Cityid = model.Cityid;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<State>> GetStatesAsync(int countryId)
        {
            return await _db.States.Where(s => s.Countryid == countryId).ToListAsync();
        }

        public async Task<List<City>> GetCitiesAsync(int stateId)
        {
            return await _db.Cities.Where(c => c.Stateid == stateId).ToListAsync();
        }

        public async Task<bool> ChangePasswordAsync(string email, string oldPassword, string newPassword)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            var hashedOldPassword = HashingHelper.ComputeSHA256(oldPassword);
            if (user.Password != hashedOldPassword) return false;

            user.Password = HashingHelper.ComputeSHA256(newPassword);
            await _db.SaveChangesAsync();
            return true;
        }
}
