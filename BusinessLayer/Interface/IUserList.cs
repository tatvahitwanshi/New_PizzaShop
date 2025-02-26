using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;

namespace BusinessLayer.Interface;

public interface IUserList
{
    List<UserListViewModel> GetUsers(string sortBy, string sortOrder);
    void AddUser(AddUserViewModel model, string email); // Method to add a user
 // Methods to fetch dropdown data
    List<Role> GetRoles();
    List<Country> GetCountries();
    List<State> GetStatesByCountry(int countryId);
    List<City> GetCitiesByState(int stateId);
    
}
