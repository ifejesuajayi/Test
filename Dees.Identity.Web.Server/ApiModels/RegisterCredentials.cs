namespace Dees.Identity.Web.Server
{
    /// <summary>
    /// The register credentials API model
    /// </summary>
    public class RegisterCredentials
    {
        /// <summary>
        /// The password of the user
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The email address of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The phone number of the user
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// The photo of user in base url encoded format
        /// </summary>
        public string Photo { get; set; }


    }
}
