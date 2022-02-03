using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineShop.OnlineClient.Identity.Models;
using System.Threading.Tasks;

namespace OnlineShop.OnlineClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<UserModel> _userManager;

        private readonly SignInManager<UserModel> _signInManager;

        public AccountController(ILogger<HomeController> logger, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register() // 123Qwe_ / zxc@xcv.com / zxc
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel user = new UserModel() { Email = model.Email, UserName = model.Email, Login = model.Login };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                   // await _userManager.AddToRoleAsync(user, "user");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginModel() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login and (or) password");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var userId = _userManager.GetUserId(User);
            UserModel user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            ChangePasswordModel model = new ChangePasswordModel() { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var passwordValidator = HttpContext.RequestServices.GetService(typeof(IPasswordValidator<UserModel>)) as IPasswordValidator<UserModel>;
                    var passwordHasher = HttpContext.RequestServices.GetService(typeof(IPasswordHasher<UserModel>)) as IPasswordHasher<UserModel>;
                    IdentityResult result = await passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, model.NewPassword);
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }

            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
