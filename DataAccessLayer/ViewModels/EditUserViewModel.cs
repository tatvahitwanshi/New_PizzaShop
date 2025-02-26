using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Models;

namespace DataAccessLayer.ViewModels;

public class EditUserViewModel
{
    [Required]
    public string? Email { get; set; }
     [Required]
    public string Username { get; set; }
    [Required]
    public string? Firstname { get; set; }
    [Required]
    public string? Lastname { get; set; }
    public bool? Isactive { get; set; } 
    public string? Profilepic { get; set; }
    public int? Zipcode { get; set; }
   [Required]
    public string? Address { get; set; }
   [Required]
    public string? Phone { get; set; }
   [Required]
    public int Countryid { get; set; }
   [Required]
    public int Stateid { get; set; }
   [Required]
    public int Cityid { get; set; }
    [Required]
    public int Roleid {get; set;}
    public string? EditedBy { get; set; }
    public DateTime? EditDate { get; set; }
    public List<Role> Roles { get; set; } = new List<Role>();  
    public List<Country> Countries { get; set; } = new List<Country>(); 
    public List<State> States { get; set; } = new List<State>(); 
    public List<City> Cities { get; set; } = new List<City>(); 
}
