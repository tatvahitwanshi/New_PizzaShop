using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;

namespace DataAccessLayer.ViewModels;

public class AddUserViewModel
{
    [Required]
    public string? Email { get; set; }
     [Required]
    public string Username { get; set; }
     [Required]
    public string Password { get; set; } = null!;
    [Required]
    public string? Firstname { get; set; }
   [Required]
    public string? Lastname { get; set; }

    public IFormFile? Profilepic { get; set; }

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
    public string? CreatedBy { get; set; }
    // Dropdown lists
    public List<Role> Roles { get; set; } = new();
    public List<Country> Countries { get; set; } = new();
    public List<State> States { get; set; } = new();
    public List<City> Cities { get; set; } = new();
    

} 
