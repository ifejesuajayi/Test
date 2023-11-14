using Dees.Identity.Web.Server.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dees.Identity.Web.Server
{
    public class EmailController : Controller
    {

        /// <summary>
        /// Get endpoint to reset a user's password
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet(WebRoutes.ResetPasswordNotificatiion)]
        public IActionResult ResetPasswordNotification([FromQuery] string returnUrl)
        {
            // Set page title
            ViewData["Title"] = "ResetPasswordNotification";

            // Create instance of LoginViewModel
            LoginViewModel lvm = new()
            {
                // Set the return url
                ReturnUrl = returnUrl
            };

            // Return the view
            return View("ResetPasswordNotification", lvm);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
