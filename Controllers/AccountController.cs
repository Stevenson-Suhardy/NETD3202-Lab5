/* Name: Stevenson Suhardy
 * Date: December 12, 2022
 * Student ID: 100839397
 */

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using ProductSellerWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProductSellerWebsite.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _logger;
        /// <summary>
        /// Constructor
        /// </summary>
        public AccountController(
                    UserManager<AppUser> userManager,
                    SignInManager<AppUser> signInManager,
                    ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }



        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion

        /// <summary>
        /// HttpGet for Register page
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// HttpPost for Register page. This will create a new user in the database
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            // Check to see if model is valid
            if (ModelState.IsValid)
            {
                // Creating a new user
                var user = new AppUser { UserName = model.Email, Email = model.Email };
                // Using usermanager to create a new user
                var result = await _userManager.CreateAsync(user, model.Password);
                // Check to see if the creation is successful
                if (result.Succeeded)
                {
                    // Log the event
                    _logger.LogInformation("User created a new account with password.");
                    // Sign in the user after registering
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");
                    // Redirecting
                    return RedirectToLocal(returnUrl);
                }
                // Display any errors during creation
                AddErrors(result);
            }

            // Redisplay the form if something went wrong
            return View(model);
        }

        /// <summary>
        /// HttpGet for Login Page
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// HttpPost for Login Page. This will log in the user based on their email and password by using the sign in manager.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            // Check to see if the model is valid
            if (ModelState.IsValid)
            {
                // Sign in using sign in manager
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                // Check to see if signed in successfully
                if (result.Succeeded)
                {
                    // Log the event
                    _logger.LogInformation("User logged in.");
                    // Redirect
                    return RedirectToLocal(returnUrl);
                }
                // Check to see if the user is locked out
                if (result.IsLockedOut)
                {
                    // Log the event
                    _logger.LogWarning("User account locked out.");
                    // Redirect to lockout
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    // If sign in failed, display error
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// HttpGet for lockout which just returns the view
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        /// <summary>
        /// returns ForgotPassword view
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// This controller is used to handle when user forgots their password and wants to reset it. For the purpose of simplicity, there are no authentication like sending email and using a token. They can just directly reset their password.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            
            // Check if the model is valid
            if (ModelState.IsValid)
            {
                // Find the user using email
                var user = await _userManager.FindByEmailAsync(model.Email);
                // check if the user is null
                if (user == null)
                {
                    // return it to the reset password page in order to not say whether user was found or not
                    return View("ResetPassword");
                }
                // Change password using old password as authentication
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
                // Check to see if it succeeded
                if (result.Succeeded)
                {
                    // This code is redundant from the change password, but when I tested this, the change password does not automatically change the user password. I had to refresh the database, so I did this instead.
                    await _userManager.RemovePasswordAsync(user);
                    await _userManager.AddPasswordAsync(user, model.Password);
                    // Return reset password view
                    return View("ResetPassword");
                }
                // Display errors
                AddErrors(result);


            }

            // return model if it fails
            return View(model);
        }

        /// <summary>
        /// Returns a view of the ResetPassword
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// HttpPost for Logging out
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Using sign in manager to sign out
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
