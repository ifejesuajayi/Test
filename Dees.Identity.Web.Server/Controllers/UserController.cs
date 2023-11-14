using Dees.Identity.Web.Server.Routes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dees.Identity.Web.Server.Controllers
{
    /// <summary>
    /// Manages web request for user operations
    /// </summary>
    public class UserController : Controller
    {
        /// <summary>
        /// Scoped instance of UserManager
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Scoped instance of SignInManager
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// Singleton instance of the <see cref="ILogger"/>
        /// </summary>
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Singleton instance of the <see cref="IConfiguration"/>
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        public UserController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, ILogger<UserController> logger, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Get endpoint to login a user
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet(WebRoutes.Login)]
        public IActionResult Login([FromQuery] string returnUrl)
        {
            // Set page title
            ViewData["Title"] = "Login";

            // Create instance of LoginViewModel
            LoginViewModel lvm = new()
            {
                // Set the return url
                ReturnUrl = returnUrl
            };

            // Return the view
            return View("Login", lvm);
        }

        /// <summary>
        /// Get endpoint to reset user's password
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet(WebRoutes.ResetPassword)]
        public IActionResult ResetPassword([FromQuery] string returnUrl)
        {
            // Set page title
            ViewData["Title"] = "Reset password";

            // Create instance of LoginViewModel
            LoginViewModel lvm = new()
            {
                // Set the return url
                ReturnUrl = returnUrl
            };

            // Return the view
            return View("ResetPassword", lvm);
        }

        /// <summary>
        /// Get endpoint to forget a user's password
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet(WebRoutes.ForgotPassword)]
        public IActionResult ForgotPassword([FromQuery] string returnUrl)
        {
            // Set page title
            ViewData["Title"] = "ForgotPassword";

            // Create instance of LoginViewModel
            LoginViewModel lvm = new()
            {
                // Set the return url
                ReturnUrl = returnUrl
            };

            // Return the view
            return View("ForgotPassword", lvm);
        }

        /// <summary>
        /// Post end point to login a user
        /// </summary>
        /// <param name="model">The <see cref="LoginViewModel"/></param>
        /// <returns></returns>
        [HttpPost(WebRoutes.Login)]
        public async Task<IActionResult> LoginAsync([FromForm] LoginViewModel model)
        {
            // If model state is valid
            if (ModelState.IsValid)
            {
                // Set the redirect url
                string redirectUrl = model.ReturnUrl ??= Url.Content("~/");

                // Get the user
                var user = await _userManager.FindByEmailAsync(model.Email);

                // If the use is null
                if (user is null)
                {
                    // Set and pass the error to view
                    ViewData["Error"] = "Invalid email or password";

                    // Return the view
                    return View("Login", model);
                }

                // Log in the user
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.IsPersistent, false);

                // If it was not successful
                if (!result.Succeeded)
                {
                    // Set and pass the error to view
                    ViewData["Error"] = "Invalid email or password";

                    // Return the view
                    return View("Login", model);
                }

                // It was a successful login
                return Redirect(redirectUrl);
            }

            // Return the view if model state is not valid
            return View("Login", model);
        }

        /// <summary>
        /// Handles sign out
        /// </summary>
        /// <returns>Redirect to the client</returns>
        [Route(WebRoutes.Logout)]
        public async Task<IActionResult> SignOutAsync()
        {
            // End the user session
            await _signInManager.SignOutAsync();

            // Return to the client
            return Redirect(_configuration["Client:Host"]);
        }
    }
}
