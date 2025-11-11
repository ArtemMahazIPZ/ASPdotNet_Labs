using Lab05.Web.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lab05.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _users;
        private readonly SignInManager<IdentityUser> _signIn;

        public AccountController(
            UserManager<IdentityUser> users,
            SignInManager<IdentityUser> signIn)
        {
            _users = users;
            _signIn = signIn;
        }

        [AllowAnonymous]
        public IActionResult Register() => View(new RegisterModel());

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var res = await _users.CreateAsync(user, model.Password);
            if (res.Succeeded)
            {
                await _users.AddToRoleAsync(user, "User");
                await _signIn.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var e in res.Errors)
                ModelState.AddModelError(string.Empty, e.Description);

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
            => View(new LoginModel { ReturnUrl = returnUrl });

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _users.FindByEmailAsync(model.Email);
            if (user != null)
            {
                await _signIn.SignOutAsync(); 
                var result = await _signIn.PasswordSignInAsync(
                    user,
                    model.Password,
                    isPersistent: false,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Невірний логін або пароль");
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signIn.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
    }
}
