namespace Dees.Identity.Web.Server
{
    /// <summary>
    /// The routes of all API endpoint in the application
    /// </summary>
    public class EndpointRoutes
    {
        /// <summary>
        /// Route to create an api resource
        /// </summary>
        public const string CreateApiResource = "api/api-resource/create";

        /// <summary>
        /// Route to create api scope
        /// </summary>
        public const string CreateApiScope = "api/api-scope/create";

        /// <summary>
        /// Route to create api scopes
        /// </summary>
        public const string CreateApiScopes = "api/api-scopes/create";

        /// <summary>
        /// Route to create identity client
        /// </summary>
        public const string CreateClient = "api/client/create";

        /// <summary>
        /// Route to create identity clients
        /// </summary>
        public const string CreateClients = "api/clients/create";

        /// <summary>
        /// Route to the create system client endpoint
        /// </summary>
        public const string CreateSystemClients = "api/system-clients/create";

        /// <summary>
        /// Route to the RegisterUser endpoint
        /// </summary>
        public const string RegisterUser = "api/user/register";

        /// <summary>
        /// Route to the UpdateUser endpoint
        /// </summary>
        public const string UpdateUser = "api/user/update/{id}";
    }
}
