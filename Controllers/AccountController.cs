using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(
                    user,
                    model.Password
                );

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(
                        user,
                        isPersistent: false
                    );

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description
                    );
                }
            }

            return View(model);
        }




        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(
                        model.Email,
                        model.Password,
                        model.RememberMe,
                        lockoutOnFailure: false
                    );

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(
                    string.Empty,
                    "Invalid login attempt."
                );
            }

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }



        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }



        [HttpGet]
        public IActionResult ChangeEmail()
        {
            return PartialView(
                "_ChangeEmail",
                new ChangeEmailViewModel()
            );
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Please enter a valid email address."
                });
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "You are not logged in."
                });
            }

            var existingUser = await _userManager.FindByEmailAsync(model.NewEmail);

            if (existingUser != null && existingUser.Id != user.Id)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "That email is already being used."
                });
            }

            var token = await _userManager.GenerateChangeEmailTokenAsync(
                user,
                model.NewEmail
            );

            var result = await _userManager.ChangeEmailAsync(
                user,
                model.NewEmail,
                token
            );

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    success = false,
                    message = string.Join(
                        ", ",
                        result.Errors.Select(e => e.Description)
                    )
                });
            }

            var usernameResult = await _userManager.SetUserNameAsync(
                user,
                model.NewEmail
            );

            if (!usernameResult.Succeeded)
            {
                return BadRequest(new
                {
                    success = false,
                    message = string.Join(
                        ", ",
                        usernameResult.Errors.Select(e => e.Description)
                    )
                });
            }

            await _signInManager.RefreshSignInAsync(user);

            return Json(new
            {
                success = true,
                message = "Email changed successfully.",
                email = user.Email,
                username = user.UserName
            });
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return PartialView(
                "_ChangePassword",
                new ChangePasswordViewModel()
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Please check the password fields."
                });
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "You are not logged in."
                });
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword
            );

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    success = false,
                    message = string.Join(
                        ", ",
                        result.Errors.Select(e => e.Description)
                    )
                });
            }

            await _signInManager.RefreshSignInAsync(user);

            return Json(new
            {
                success = true,
                message = "Password changed successfully."
            });
        }
    }
}