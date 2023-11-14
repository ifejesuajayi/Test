using Dees.Identity.Web.Server.Routes;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Dees.Identity.Web.Server
{
    /// <summary>
    /// Application extensions
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// This will apply migrations to configuration db
        /// </summary>
        /// <param name="app"></param>
        public static void ApplyMigrations(this WebApplication app)
        {
            // Create scoped instance
            using var scope = app.Services.CreateScope();

            // Get the service provider
            var service = scope.ServiceProvider;

            // Get the logger
            var loggerFactory = service.GetRequiredService<ILoggerFactory>();

            try
            {
                // Get the identity database context
                var identityContext = service.GetRequiredService<ApplicationDbContext>();

                // Get the configuration database context
                var configContext = service.GetRequiredService<ConfigurationDbContext>();

                // Migrate the identity database
                identityContext.Database.Migrate();

                // Migrate the configuration database
                configContext.Database.Migrate();

                // Create identity resources
                if (!configContext.IdentityResources.Any())
                {
                    // Set the identity resources credentials
                    var identityResources = new List<IdentityResource>
                    {
                        new IdentityResources.OpenId(),
                        new IdentityResources.Email(),
                        new IdentityResources.Profile()
                    };

                    // For each resource credential...
                    foreach (var resource in identityResources)
                    {
                        // Create the resource
                        configContext.IdentityResources.Add(resource.ToEntity());
                    }

                    // Save changes
                    configContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Create instance of ILogger
                var logger = loggerFactory.CreateLogger<Program>();

                // Log the error
                logger.LogError("An error occurred. Details: {error}", ex.Message);
            }
        }

        /// <summary>
        /// Adding cors to the IOC Container
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            // Add cors
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", options =>
                {
                    options.AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowAnyMethod();
                });
            });

            // Return services for further chaining
            return services;
        }

        /// <summary>
        /// Add identity to IOC container
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            // Register identity as a service
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Return services for further chaining
            return services;
        }

        /// <summary>
        /// Add Identity server to IOC
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureIdentityServer(this IServiceCollection services, IConfiguration config)
        {
            // Add identity server service
            services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = WebRoutes.Login;
                options.UserInteraction.LogoutUrl = WebRoutes.Logout;
                options.UserInteraction.ErrorUrl = WebRoutes.Error;
            })
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>()
                // TODO: Add ConfigurationDbStoreCache to reduce load and requests to the database
                .AddConfigurationStore<ConfigurationDbContext>(options =>
                {
                    options.ConfigureDbContext = option => option.UseSqlServer(config.GetConnectionString("ConfigurationConnection"),
                    migration => migration.MigrationsAssembly(typeof(Program).Assembly.GetName().Name));
                });

            // Return services for further chaining
            return services;
        }

        /// <summary>
        /// Add Authentication to IOC container
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication()
                .AddIdentityServerJwt()
                .AddJwtBearer(options =>
                {
                    options.Authority = config["Jwt:Authority"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidTypes = new[] { "at+jwt" }
                    };
                });

            // Return services for further chaining
            return services;
        }

        /// <summary>
        /// Configure auth cookie behavior
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureCookieBehavior(this IServiceCollection services)
        {
            // Configure the application cookie
            services.ConfigureApplicationCookie(options =>
            {
                // Set the login path user will be redirected to
                options.LoginPath = WebRoutes.Login;
            });

            // Return services for further chaining
            return services;
        }
    }
}
