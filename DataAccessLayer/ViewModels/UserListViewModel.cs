namespace DataAccessLayer.ViewModels;

public class UserListViewModel
{
    private object value;

    public int Userid { get; set; }
 
    public string? Firstname { get; set;}
    
    public string? Lastname {get; set;}

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? RoleName {get; set; }

    public bool? Isactive { get; set; }
}
