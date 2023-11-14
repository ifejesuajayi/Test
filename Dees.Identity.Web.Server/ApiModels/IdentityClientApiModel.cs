namespace Dees.Identity.Web.Server
{
    public class IdentityClientApiModel
    {
        /// <summary>
        /// The uri for this client
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// The id of this client
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The name of this client
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The scope of this client
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// The api secret of this client
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// The grant types for this client
        /// </summary>
        public string[] GrantTypes { get; set; }

        /// <summary>
        /// The redirectUris for this client
        /// </summary>
        public string[] RedirectUris { get; set; }

        /// <summary>
        /// The additional claims for this user
        /// </summary>
        public Dictionary<string, string> AdditionalClaims { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// The post log out redirect uris
        /// </summary>
        public string[] PostLogoutRedirectUris { get; set; }
    }
}
