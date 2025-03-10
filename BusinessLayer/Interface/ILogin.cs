
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;

namespace BusinessLayer.Interface;

public interface ILogin
{
    Task<(User, string)> AuthenticateUserAsync(string email, string password);
    Task<string> GenerateJwtTokenAsync(string email, int roleId=0, HttpResponse response=null, bool rememberMe=false);
    Task<string> ResetEmailToken(string email,int roleId=0, HttpResponse response=null, bool rememberMe=false);
     Task<bool> ForgotPasswordAsync(ForgetPasswordViewModel model , string callbackUrl);
    Task<bool> ResetPasswordAsync(string email, string newPassword);
    Task<TokenViewModel> ValidateToken(string token1);
    Task<string> GetRoleName(int roleId);

}
