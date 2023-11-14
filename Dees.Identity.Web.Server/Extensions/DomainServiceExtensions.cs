namespace Dees.Identity.Web.Server
{
    /// <summary>
    /// The extensions for domain services
    /// </summary>
    public static class DomainServiceExtensions
    {
        /// <summary>
        /// Adds <see cref="IdentityOperations"/> to DI container
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <returns>The <see cref="IServiceCollection"/> for further chaining</returns>
        public static IServiceCollection AddIdentityOperations(this IServiceCollection services)
        {
            // Add scoped instance identity operations
            services.AddScoped<IdentityOperations>();

            // Return services for further chaining
            return services;
        }
    }
}
