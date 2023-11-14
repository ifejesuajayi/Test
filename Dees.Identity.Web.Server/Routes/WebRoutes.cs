namespace Dees.Identity.Web.Server.Routes
{
    /// <summary>
    /// The Web Routes in the application
    /// </summary>
    public class WebRoutes
    {
        /// <summary>
        /// The route to the register endpoint
        /// </summary>
        public const string Register = "/user/register";

        /// <summary>
        /// The route to the login endpoint
        /// </summary>
        public const string Login = "/user/login";

        /// <summary>
        /// The route to the ForgotPassword endpoint
        /// </summary>
        public const string ForgotPassword = "/user/forgotpassword";

        /// <summary>
        /// The route to the ResetPassword endpoint
        /// </summary>
        public const string ResetPasswordNotificatiion = "/email/resetpassword";

        /// <summary>
        /// The route to the ResetPassword endpoint
        /// </summary>
        public const string ResetPassword = "/user/reset-password";

        /// <summary>
        /// The route to the logout endpoint
        /// </summary>
        public const string Logout = "/user/logout";

        /// <summary>
        /// The route to the error endpoint
        /// </summary>
        public const string Error = "/error";
    }
}
