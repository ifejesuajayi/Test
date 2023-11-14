namespace Dees.Identity.Web.Server
{
    /// <summary>
    /// The api resource api model
    /// </summary>
    public class ApiResourceApiModel
    {
        /// <summary>
        /// The api resource name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The scopes asssoicated with this api resource
        /// </summary>
        public string[] Scopes { get; set; }

        /// <summary>
        /// The display name for this api resource
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The api resource description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The api secret associated with this resource
        /// </summary>
        public string ApiSecret { get; set; }
    }
}
