namespace Dees.Identity.Web.Server
{
    /// <summary>
    /// The api scope model
    /// </summary>
    public class ApiScopeApiModel
    {
        /// <summary>
        /// The name of this api scope
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The display name of the scope
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The description of the scope
        /// </summary>
        public string Description { get; set; }
    }
}
