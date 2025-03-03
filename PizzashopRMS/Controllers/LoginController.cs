using BusinessLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Threading.Tasks;


namespace PizzaShopApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogin _loginRepository;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogin loginRepository, ILogger<LoginController> logger)
        {
            _loginRepository = loginRepository;
            _logger = logger;
        }

        public IActionResult LoginView()
        {
            String req_cookie = Request.Cookies["UserEmail"];
            if (!String.IsNullOrEmpty(req_cookie))
            {
                return RedirectToAction("DashboardView", "Dashboard");
            }
            return View();
        }

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

                var token = await _loginRepository.GenerateJwtTokenAsync(dbUser.Email, dbUser.Roleid, Response, user.RememberMe);

                TempData["success"] = message;
                return RedirectToAction("DashboardView", "Dashboard");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login.");
                throw; // Rethrow exception to be handled globally
            }
        }

        public IActionResult ForgotPasswordView(string email)
        {
            ViewData["email"] = string.IsNullOrEmpty(email) ? "" : email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("ForgotPasswordView", model);
                }

                var callbackUrl = Url.ActionLink("ResetPasswordView", "Login");
                bool isEmailSent = await _loginRepository.ForgotPasswordAsync(model, callbackUrl);

                if (isEmailSent)
                {
                    TempData["success"] = "A password reset link has been sent to your email.";
                }
                else
                {
                    TempData["error"] = "Email not found!";
                }

                var token = await _loginRepository.GenerateJwtTokenAsync(model.Email);
                return View("LoginView");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ForgotPassword.");
                throw; // Rethrow exception
            }
        }

        public IActionResult ResetPasswordView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
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

        public IActionResult Logout()
        {
            Response.Cookies.Delete("JWTLogin");
            Response.Cookies.Delete("UserEmail");
            Response.Cookies.Delete("UserRole");
            Response.Cookies.Delete("UserName");
            return View("LoginView");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
