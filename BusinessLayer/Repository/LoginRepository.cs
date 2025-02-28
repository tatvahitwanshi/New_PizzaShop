using System;
using BusinessLayer.Helper;
using BusinessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PizzaShopApp.Helpers;

namespace BusinessLayer.Repository;

public class LoginRepository : ILogin
{
    private readonly PizzaShopContext _db;
    private readonly GenerateJwtTokenHelper _jwtTokenHelper;
    private readonly IEmailService _emailService; // Assuming you have an EmailService

    public LoginRepository(PizzaShopContext db, GenerateJwtTokenHelper jwtTokenHelper, IEmailService emailService)
    {
        _db = db;
        _jwtTokenHelper = jwtTokenHelper;
        _emailService = emailService;

    }


    public async Task<(User, string)> AuthenticateUserAsync(string email, string password)
    {
        var user = await Task.Run(() => _db.Users.FirstOrDefault(u => u.Email == email));
        if (user == null) return (null, "User Does not exist");

        var hashedPassword = HashingHelper.ComputeSHA256(password);
        return user.Password == hashedPassword ? (user, "Login Successfull") : (null,"Wrong password");
    }

    public async Task<string> GenerateJwtTokenAsync(string email, int roleId, HttpResponse response, bool rememberMe)
    {
        var userRole = _db.Roles.FirstOrDefault(r => r.Roleid == roleId);
        if (userRole == null) return null;

        var token = _jwtTokenHelper.GenerateJwtToken(email, userRole.Rolename);

        response.Cookies.Append("JWTLogin", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        if (rememberMe)
        {
            response.Cookies.Append("UserEmail", email, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(30)
            });
        }

        return token;
    }
    public async Task<bool> ForgotPasswordAsync(ForgetPasswordViewModel model, string callbackUrl)
    {
        if (model == null)
            return false;

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (user == null)
            return false;



        string subject = "Password Reset Request";
        string message = @$"
                <div style='padding: 20px; background-color: #0066A7; display: flex; justify-content: center;'>
                    <h1 style='align-items: center; color:white'>PIZZASHOP</h1>
                </div>
                <div style='background-color: rgba(128, 128, 128, 0.158); padding: 3%;'>
                    <span><br> Pizza shop, <br><br> 
                    Please click <a href='{callbackUrl}'>here</a> to reset your password. <br><br>
                    If you encounter any issues, please contact our support team. <br>
                    <span style='color: yellow;'>Important Note:</span> This link will expire in 24 hours. If you did not request a password reset, ignore this email. <br><br>    
                    </span>
                </div>";

        return await _emailService.SendEmailAsync(user.Email, subject, message);
    }
    public async Task<bool> ResetPasswordAsync(string email, string newPassword)
    {
        try
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;
            }

            user.Password = HashingHelper.ComputeSHA256(newPassword); // Hash password
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }


}
