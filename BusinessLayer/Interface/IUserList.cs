using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;

namespace BusinessLayer.Interface;

public interface IUserList
{
    // Task<(List<UserListViewModel>, int, int, int, string, string, string)> GetUsers(int PageSize, int PageNumber, string sortBy, string sortOrder, string SearchKey);
    Task<(List<UserListViewModel> UserList, int Count, int PageSize, int PageNumber, string SortBy, string SortOrder, string SearchKey)>GetUsers(int PageSize, int PageNumber, string sortBy, string sortOrder, string SearchKey);
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
