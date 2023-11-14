using Microsoft.AspNetCore.Identity;

namespace Dees.Identity.Web.Server
{
    /// <summary>
    /// The user model
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// The user's account id
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// The first name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The point in time user was created
        /// </summary>
        public DateTimeOffset DateCreated { get; set; }

        /// <summary>
        /// The point in time user was modified
        /// </summary>
        public DateTimeOffset DateModified { get; set; }
    }
}
