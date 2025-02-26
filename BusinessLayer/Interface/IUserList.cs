using DataAccessLayer.ViewModels;

namespace BusinessLayer.Interface;

public interface IUserList
{
    List<UserListViewModel> GetUsers(string sortBy, string sortOrder);

}
