using Duende.IdentityServer.Models;
using System.ComponentModel.DataAnnotations;

namespace Dees.Identity.Web.Server
{
    /// <summary>
    /// The login view model
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// The email of the user
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "The specified email is not a valid email address")]
        public string? Email { get; set; }

        /// <summary>
        /// The password of the user
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        /// <summary>
        /// False if login session is to be forgotten
        /// </summary>
        public bool IsPersistent { get; set; }

        /// <summary>
        /// The redirect url
        /// </summary>
        public string? ReturnUrl { get; set; }
    }
}
