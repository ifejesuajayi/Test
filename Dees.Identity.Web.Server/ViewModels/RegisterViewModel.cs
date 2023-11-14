using System.ComponentModel.DataAnnotations;

namespace Dees.Identity.Web.Server
{
    /// <summary>
    /// The register view model 
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// The first name of the user
        /// </summary>
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user
        /// </summary
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        /// <summary>
        /// The email of the user
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// The phone of the user
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// The password of the user
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// The scope of the user
        /// </summary>
        public string Scope { get; set; }
    }
}
