using Dees.Identity.Web.Server;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;

namespace Dees.Identity.Web.Server
{
    /// <summary>
    /// The scoped instance of the <see cref="ApplicationDbContext"/>
    /// </summary>
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="options">The database context options</param>
        /// <param name="operationalStoreOptions">The operational store options</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        /// <summary>
        /// Override the save changes async
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Check the entries to db
            foreach (var entry in ChangeTracker.Entries<ApplicationUser>())
            {
                // Set the date modified
                entry.Entity.DateModified = DateTimeOffset.Now;

                // If a record is being added
                if (entry.State == EntityState.Added)
                {
                    // Set the date created
                    entry.Entity.DateCreated = DateTimeOffset.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Configures the schema needed for the identity framework
        /// </summary>
        /// <param name="builder">The model builder</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Configure unique index for user's account id
            builder.Entity<ApplicationUser>().HasIndex(u => u.AccountId).IsUnique();

            base.OnModelCreating(builder);
        }
    }
}
