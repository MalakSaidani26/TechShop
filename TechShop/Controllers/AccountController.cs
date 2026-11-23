using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TechShop.Models;
using TechShop.ViewModels;

namespace TechShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var languageCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = _authService.Login(model.Email, model.Password);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, languageCode switch
                {
                    "en" => "Invalid email or password.",
                    "ar" => "البريد الإلكتروني أو كلمة المرور غير صحيحة.",
                    _ => "Email ou mot de passe invalide."
                });
                return View(model);
            }

            return RedirectToLocal(model.ReturnUrl);
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            var languageCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = _authService.Register(model.FullName, model.Email, model.Password);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, languageCode switch
                {
                    "en" => "An account with this email already exists.",
                    "ar" => "يوجد حساب بهذا البريد الإلكتروني بالفعل.",
                    _ => "Un compte avec cet email existe déjà."
                });
                return View(model);
            }

            return RedirectToLocal(model.ReturnUrl);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _authService.Logout();
            return RedirectToAction("List", "Product");
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("List", "Product");
        }
    }
}
