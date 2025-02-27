using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;

namespace BusinessLayer.Interface;

public interface IUserList
{
    List<UserListViewModel> GetUsers(string sortBy, string sortOrder);
    void AddUser(AddUserViewModel model, string email); // Method to add a user
    
    Task<bool> AddUserEmail(string newEmail, string callbackUrl);

    List<Role> GetRoles();
    List<Country> GetCountries();
    List<State> GetStatesByCountry(int countryId);
    List<City> GetCitiesByState(int stateId);
    Task<EditUserViewModel> GetUserProfileDetailsAsync(int userId);
    Task<bool> EditUserProfileDetailsAsync(EditUserViewModel model);
    Task DeleteUser(int userId);
}
