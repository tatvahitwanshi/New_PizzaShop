using BusinessLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Threading.Tasks;
using DataAccessLayer.Models;


namespace PizzaShopApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogin _loginRepository;
        private readonly ILogger<LoginController> _logger;

        // Constructor to initialize login repository and logger
        public LoginController(ILogin loginRepository, ILogger<LoginController> logger)
        {
            _loginRepository = loginRepository;
            _logger = logger;
        }

        // Displays the login page, redirects if user email cookie exists
        public IActionResult LoginView()
        {

            String req_cookie = Request.Cookies["UserEmail"];
            if (!String.IsNullOrEmpty(req_cookie))
            {
                return RedirectToAction("DashboardView", "Dashboard");
            }

            return View();
        }

        // Handles user login authentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("LoginView", user);
                }

                var (dbUser, message) = await _loginRepository.AuthenticateUserAsync(user.Email, user.Password);

                if (dbUser == null)
                {
                    TempData["error"] = message;
                    return RedirectToAction("LoginView", "Login");
                }
                var role = await _loginRepository.GetRoleName(dbUser.Roleid);
                var token = await _loginRepository.GenerateJwtTokenAsync(dbUser.Email, dbUser.Roleid, Response, user.RememberMe);
                HttpContext.Session.SetString("Email", dbUser.Email);
                HttpContext.Session.SetString("ProfilePic", dbUser.Profilepic ?? "~/images/Default_pfp.svg.png"); // Default if null
                await HttpContext.Session.CommitAsync();

                if (role == "admin")
                {
                    TempData["success"] = message;
                    return RedirectToAction("DashboardView", "Dashboard");
                }
                else if (role == "chef")
                {
                    TempData["success"] = message;
                    return RedirectToAction("DashboardChefView", "DashboardChef");
                }
                else if (role == "account manager")
                {
                    TempData["success"] = message;
                    return RedirectToAction("DashboardAccountManagerView", "DashboardAccountManager");
                }
                else
                {
                    TempData["error"] = "Invalid role.";
                    return RedirectToAction("LoginView", "Login");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login.");
                throw;
            }
        }

        // Displays the forgot password page
        public IActionResult ForgotPasswordView(string email)
        {
            ViewData["email"] = string.IsNullOrEmpty(email) ? "" : email;
            return View();
        }

        // Forgot password post method
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("ForgotPasswordView", model);
                }
                var token1 = await _loginRepository.ResetEmailToken(model.Email, 0, Response, false);
                TempData["Email"] = model.Email;

                var callbackUrl = Url.ActionLink("ResetPasswordView", "Login", new { token1 = token1 });
                bool isEmailSent = await _loginRepository.ForgotPasswordAsync(model, callbackUrl);

                if (isEmailSent)
                {
                    TempData["success"] = "A password reset link has been sent to your email.";
                }
                else
                {
                    TempData["error"] = "Email not found!";
                }

                return View("LoginView");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ForgotPassword.");
                throw; // Rethrow exception
            }
        }

        // Displays the reset password page
        public IActionResult ResetPasswordView(string token1)
        {
            ViewData["ResetPasswordEmail"] = token1;
            return View();
        }

        //Reset password post method
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                var token1 = model.token1;
                var validate = await _loginRepository.ValidateToken(token1);
                if (!validate.valid)
                {
                    TempData["error"] = "Link got expired generate new link";
                    return RedirectToAction("ForgotPasswordView", "Login");
                }
                if (!ModelState.IsValid)
                {
                    return View("ResetPasswordView", model);
                }

                var email = TempData["email"] as string;
                if (string.IsNullOrEmpty(email))
                {
                    ModelState.AddModelError("", "Invalid or expired request.");
                    return View("ResetPasswordView", model);
                }

                var result = await _loginRepository.ResetPasswordAsync(email, model.Password);

                if (result)
                {
                    TempData["success"] = "Password reset successfully. Please login.";
                    return RedirectToAction("LoginView");
                }
                else
                {
                    ModelState.AddModelError("", "Email not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password reset.");
                throw; // Rethrow the exception
            }
            return View("ResetPasswordView", model);
        }

        // Logs out the user by clearing authentication cookies
        public IActionResult Logout()
        {
            Response.Cookies.Delete("JWTLogin");
            Response.Cookies.Delete("UserEmail");
            Response.Cookies.Delete("UserRole");
            Response.Cookies.Delete("UserName");
            return View("LoginView");
        }

        // Displays access denied page
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
