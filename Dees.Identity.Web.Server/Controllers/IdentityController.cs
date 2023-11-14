using Microsoft.AspNetCore.Mvc;

namespace Dees.Identity.Web.Server
{
    [ApiController]
    public class IdentityController : ControllerBase
    {
        #region Private Members

        /// <summary>
        /// The scoped instance of Identity server domain
        /// </summary>
        private readonly IdentityOperations identityOperations;

        /// <summary>
        /// The singleton instance of the <see cref="ILogger{TCategoryName}"/>
        /// </summary>
        private readonly ILogger<IdentityController> logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="identityOperations">The injected <see cref="IdentityOperations"/></param>
        public IdentityController(IdentityOperations identityOperations, ILogger<IdentityController> logger)
        {
            this.identityOperations = identityOperations;
            this.logger = logger;
        }

        #endregion

        /// <summary>
        /// Endpoint to create api scope
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(EndpointRoutes.CreateApiScope)]
        public async Task<ActionResult> CreateApiScopeAsync([FromBody] ApiScopeApiModel model)
        {
            try
            {
                // Invoke the operation
                var operation = await identityOperations.CreateApiScopeAsync(model);

                // If operation failed...
                if (!operation.Successful)
                {
                    // Return error response
                    return Problem(title: operation.ErrorTitle,
                        statusCode: operation.StatusCode, detail: operation.ErrorMessage);
                }

                // Return response
                return Created(string.Empty, operation.Result);
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR",
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }

        /// <summary>
        /// Endpoint to create list of api scopes
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost(EndpointRoutes.CreateApiScopes)]
        public async Task<ActionResult> CreateApiScopesAsync([FromBody] List<ApiScopeApiModel> models)
        {
            try
            {
                // Invoke the operation
                var operation = await identityOperations.CreateApiScopesAsync(models);

                // If operation failed...
                if (!operation.Successful)
                {
                    // Return error response
                    return Problem(title: operation.ErrorTitle,
                        statusCode: operation.StatusCode, detail: operation.ErrorMessage);
                }

                // Return response
                return Created(string.Empty, operation.Result);
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR",
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }

        /// <summary>
        /// Endpoint to create api resource 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(EndpointRoutes.CreateApiResource)]
        public async Task<ActionResult> CreateApiResourceAsync([FromBody] ApiResourceApiModel model)
        {
            try
            {
                // Invoke the operation
                var operation = await identityOperations.CreateApiResourceAsync(model);

                // If operation failed...
                if (!operation.Successful)
                {
                    // Return error response
                    return Problem(title: operation.ErrorTitle,
                        statusCode: operation.StatusCode, detail: operation.ErrorMessage);
                }

                // Return response
                return Created(string.Empty, operation.Result);
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR",
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }

        /// <summary>
        /// Endpoint to create identity client
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(EndpointRoutes.CreateClient)]
        public async Task<ActionResult> CreateClientAsync([FromBody] IdentityClientApiModel model)
        {
            try
            {
                // Invoke the operation
                var operation = await identityOperations.CreateClientAsync(model);

                // If operation failed...
                if (!operation.Successful)
                {
                    // Return error response
                    return Problem(title: operation.ErrorTitle,
                        statusCode: operation.StatusCode, detail: operation.ErrorMessage);
                }

                // Return response
                return Created(string.Empty, operation.Result);
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR",
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }

        /// <summary>
        /// Endpoint to create identity client
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost(EndpointRoutes.CreateClients)]
        public async Task<ActionResult> CreateClientsAsync([FromBody] List<IdentityClientApiModel> models)
        {
            try
            {
                // Invoke the operation
                var operation = await identityOperations.CreateClientsAsync(models);

                // If operation failed...
                if (!operation.Successful)
                {
                    // Return error response
                    return Problem(title: operation.ErrorTitle,
                        statusCode: operation.StatusCode, detail: operation.ErrorMessage);
                }

                // Return response
                return Created(string.Empty, operation.Result);
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR",
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }

        /// <summary>
        /// Endpoint to create identity clients with code grant type
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost(EndpointRoutes.CreateSystemClients)]
        public async Task<ActionResult> CreateSystemClientsAsync([FromBody] List<IdentityClientApiModel> models)
        {
            try
            {
                // Invoke the operation
                var operation = await identityOperations.CreateSystemClientsAsync(models);

                // If operation failed...
                if (!operation.Successful)
                {
                    // Return error response
                    return Problem(title: operation.ErrorTitle,
                        statusCode: operation.StatusCode, detail: operation.ErrorMessage);
                }

                // Return response
                return Created(string.Empty, operation.Result);
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR",
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }
    }
}
