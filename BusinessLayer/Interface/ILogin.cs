
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;

namespace BusinessLayer.Interface;

public interface ILogin
{
    Task<User> AuthenticateUserAsync(string email, string password);
    Task<string> GenerateJwtTokenAsync(string email, int roleId, HttpResponse response, bool rememberMe);
     Task<bool> ForgotPasswordAsync(ForgetPasswordViewModel model , string callbackUrl);
    Task<bool> ResetPasswordAsync(string email, string newPassword);
}
