using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Duende.IdentityServer;
using IdentityModel;

namespace Dees.Identity.Web.Server
{
    /// <summary>
    /// The identity server related operations
    /// </summary>
    public class IdentityOperations
    {
        #region Private Members

        /// <summary>
        /// The scoped instance of the <see cref="ApplicationDbContext"/>
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// The scoped instance of the <see cref="ConfigurationDbContext"/>
        /// </summary>
        private readonly ConfigurationDbContext _configurationContext;

        /// <summary>
        /// The singleton instance of the <see cref="IdentityOperations"/>
        /// </summary>
        private readonly ILogger<IdentityOperations> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// <param name="context">The injected context</param>
        /// <param name="logger">The injected logger</param>
        /// </summary>
        public IdentityOperations(ApplicationDbContext context, ILogger<IdentityOperations> logger, ConfigurationDbContext configurationContext)
        {
            _context = context;
            _logger = logger;
            _configurationContext = configurationContext;
        }

        #endregion

        /// <summary>
        /// Handles request to create api scope
        /// </summary>
        /// <param name="scopeCredentials"></param>
        /// <returns></returns>
        public async Task<OperationResult> CreateApiScopeAsync(ApiScopeApiModel scopeCredentials)
        {
            try
            {
                // Set the api scope credentials
                var apiScope = new Duende.IdentityServer.Models.ApiScope
                {
                    Name = scopeCredentials.Name,
                    DisplayName = scopeCredentials.DisplayName,
                    Description = scopeCredentials.Description,
                    ShowInDiscoveryDocument = false
                };

                // Create the api scope
                await _configurationContext.ApiScopes.AddAsync(apiScope.ToEntity());

                // Save changes
                var succeeded = await _configurationContext.SaveChangesAsync() > 0;

                // If failed...
                if (!succeeded)
                {
                    // Throw exception
                    throw new Exception("Operation failed due to database error");
                }

                // Return result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (Exception ex)
            {
                // Return error result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorTitle = "SYSTEM ERROR",
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Creates an api scope with supplied credentials
        /// </summary>
        /// <param name="scopes">The API scopes</param>
        public async Task<OperationResult> CreateApiScopesAsync(List<ApiScopeApiModel> scopes)
        {
            try
            {
                // For each scope item...
                foreach (var scope in scopes)
                {
                    // Create the api scope
                    await _configurationContext.ApiScopes.AddAsync(new Duende.IdentityServer.Models.ApiScope
                    {
                        Name = scope.Name,
                        DisplayName = scope.DisplayName,
                        Description = scope.Description,
                        ShowInDiscoveryDocument = false
                    }.ToEntity());
                }

                // Save changes
                var succeeded = await _configurationContext.SaveChangesAsync() > 0;

                // If failed...
                if (!succeeded)
                {
                    // Throw exception
                    throw new Exception("Operation failed due to database error");
                }

                // Return result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (Exception ex)
            {
                // Return error result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorTitle = "SYSTEM ERROR",
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Creates an api resource
        /// </summary>
        /// <param name="resourceCredentials">The api resource credentials</param>
        public async Task<OperationResult> CreateApiResourceAsync(ApiResourceApiModel resourceCredentials)
        {
            try
            {
                // Set the api resource credentials
                var apiResource = new Duende.IdentityServer.Models.ApiResource
                {
                    Name = resourceCredentials.Name,
                    DisplayName = resourceCredentials.DisplayName,
                    Description = resourceCredentials.Description,
                    Scopes = resourceCredentials.Scopes,
                    ApiSecrets = new List<Duende.IdentityServer.Models.Secret> { new Duende.IdentityServer.Models.Secret(resourceCredentials.ApiSecret.Sha256()) },
                    UserClaims = new List<string> { JwtClaimTypes.Subject }
                };

                // Create new api resource
                await _configurationContext.ApiResources.AddAsync(apiResource.ToEntity());

                // Save changes
                var succeeded = await _configurationContext.SaveChangesAsync() > 0;

                // If failed...
                if (!succeeded)
                {
                    // Throw exception
                    throw new Exception("Operation failed due to database error");
                }

                // Return result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (Exception ex)
            {
                // Return error result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorTitle = "SYSTEM ERROR",
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Creates client for the identity server
        /// </summary>
        /// <param name="clientCredentials">The identity client credentials</param>
        public async Task<OperationResult> CreateClientAsync(IdentityClientApiModel clientCredentials)
        {
            try
            {
                // Set the client credentials
                var client = new Duende.IdentityServer.Models.Client
                {
                    ClientId = clientCredentials.Id,
                    ClientUri = clientCredentials.Uri,
                    ClientName = clientCredentials.Name,
                    ClientSecrets = { new Duende.IdentityServer.Models.Secret(clientCredentials.Secret.Sha256()) },

                    AllowedGrantTypes = clientCredentials.GrantTypes,

                    RedirectUris = clientCredentials?.RedirectUris,

                    PostLogoutRedirectUris = clientCredentials?.PostLogoutRedirectUris,

                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 84400,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Profile,
                        clientCredentials.Scope
                    },
                    AlwaysIncludeUserClaimsInIdToken = true
                };

                // Create the client
                await _configurationContext.Clients.AddAsync(client.ToEntity());

                // Save changes
                var succeeded = await _configurationContext.SaveChangesAsync() > 0;

                // If failed...
                if (!succeeded)
                {
                    // Throw exception
                    throw new Exception("Operation failed due to database error");
                }

                // Return result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (Exception ex)
            {
                // Return error result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorTitle = "SYSTEM ERROR",
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Creates multiple clients for the identity server
        /// </summary>
        /// <param name="clients">The identity client credentials</param>
        public async Task<OperationResult> CreateClientsAsync(List<IdentityClientApiModel> clients)
        {
            try
            {
                // For each client credential...
                foreach (var credential in clients)
                {
                    // Initialize the client claims
                    var clientClaims = new List<ClientClaim>();

                    // For each additional claims...
                    foreach (var claim in credential.AdditionalClaims)
                    {
                        // Set the key and value
                        clientClaims.Add(new ClientClaim
                        {
                            Type = claim.Key,
                            Value = claim.Value
                        });
                    }

                    // Set the client credentials
                    var client = new Duende.IdentityServer.Models.Client
                    {
                        ClientId = credential.Id,
                        ClientUri = credential.Uri,
                        ClientName = credential.Name,
                        ClientSecrets = { new Duende.IdentityServer.Models.Secret(credential.Secret.Sha256()) },

                        AllowedGrantTypes = GrantTypes.Code,

                        RedirectUris = credential.RedirectUris,

                        PostLogoutRedirectUris = credential.PostLogoutRedirectUris,

                        AllowOfflineAccess = true,
                        AccessTokenLifetime = 84400,
                        AllowedScopes = new List<string>
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Email,
                            IdentityServerConstants.StandardScopes.Profile,
                            credential.Scope
                        },
                        Claims = clientClaims,
                        AlwaysIncludeUserClaimsInIdToken = true
                    };

                    // Create the client
                    await _configurationContext.Clients.AddAsync(client.ToEntity());
                }

                // Save changes
                var succeeded = await _configurationContext.SaveChangesAsync() > 0;

                // If failed...
                if (!succeeded)
                {
                    // Throw exception
                    throw new Exception("Operation failed due to database error");
                }

                // Return result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (Exception ex)
            {
                // Return error result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorTitle = "SYSTEM ERROR",
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Creates multiple clients for the identity server
        /// </summary>
        /// <param name="clientCredentials">The identity client credentials</param>
        public async Task<OperationResult> CreateSystemClientsAsync(List<IdentityClientApiModel> clients)
        {
            try
            {
                // For each client credential...
                foreach (var credential in clients)
                {
                    // Initialize the client claims
                    var clientClaims = new List<ClientClaim>();

                    // For each additional claims...
                    foreach (var claim in credential.AdditionalClaims)
                    {
                        // Set the key and value
                        clientClaims.Add(new ClientClaim
                        {
                            Type = claim.Key,
                            Value = claim.Value
                        });
                    }

                    // Set the client credentials
                    var client = new Duende.IdentityServer.Models.Client
                    {
                        ClientId = credential.Id,
                        ClientUri = credential.Uri,
                        ClientName = credential.Name,
                        ClientSecrets = { new Duende.IdentityServer.Models.Secret(credential.Secret.Sha256()) },

                        AllowedGrantTypes = GrantTypes.ClientCredentials,

                        AllowOfflineAccess = true,
                        AccessTokenLifetime = 84400,
                        AllowedScopes = { credential.Scope },
                        Claims = clientClaims,
                        AlwaysIncludeUserClaimsInIdToken = true
                    };

                    // Create the client
                    await _configurationContext.Clients.AddAsync(client.ToEntity());
                }

                // Save changes
                var succeeded = await _configurationContext.SaveChangesAsync() > 0;

                if (!succeeded)
                {
                    // Throw exception
                    throw new Exception("Operation failed due to database error");
                }

                // Return result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (Exception ex)
            {
                // Return error result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorTitle = "SYSTEM ERROR",
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
