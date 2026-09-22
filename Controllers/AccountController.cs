using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem.Models;
using ProductManagementSystem.Services;
using System.Security.Claims;

namespace ProductManagementSystem.Controllers
{
    public class AccountController : Controller
    {
private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadProfilePhoto(IFormFile? photo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { success = false, message = "You are not logged in." });
            }

            var result = await _userService.UploadProfilePhotoAsync(userId, photo);

            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.ErrorMessage });
            }

            var user = await _userService.GetUserByIdAsync(userId);

            return Json(new
            {
                success = true,
                photoUrl = user?.ProfilePhotoUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProfilePhoto()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { success = false, message = "You are not logged in." });
            }

            var deleted = await _userService.DeleteProfilePhotoAsync(userId);

            return Json(new
            {
                success = deleted,
                message = deleted
                    ? "Profile photo removed successfully."
                    : "Profile photo could not be removed."
            });
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "You are not logged in."
                });
            }

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "You are not logged in."
                });
            }

            var result = await _userService.ChangeEmailAsync(
                user,
                model.NewEmail
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
        public async Task<IActionResult> ChangePassword(
      ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Please check the password fields."
                });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "You are not logged in."
                });
            }

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "You are not logged in."
                });
            }

            var result = await _userService.ChangePasswordAsync(
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
