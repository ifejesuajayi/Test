using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Dees.Identity.Web.Server
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Scoped instance of UserManager
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// The scoped instance of application db context
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// The singleton instance of the <see cref="ILogger{TCategoryName}"/>
        /// </summary>
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Post endpoint to register a user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>sa
        [HttpPost(EndpointRoutes.RegisterUser)]
        public async Task<ActionResult> RegisterUserAsync([FromBody] RegisterViewModel model)
        {
            try
            {
                // If email exist...
                if (await _context.Users.AnyAsync(user => user.Email == model.Email))
                {
                    // Return error response
                    return Problem(title: "BAD REQUEST",
                        statusCode: StatusCodes.Status400BadRequest, detail: "The specified email already exist");
                }

                // Generate the account id
                var accountId = await GenerateAccountId();

                // Create the user
                var result = await _userManager.CreateAsync(new ApplicationUser
                {
                    AccountId = accountId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = accountId,
                    PhoneNumber = model.Phone
                }, model.Password);

                // If it was not successful
                if (!result.Succeeded)
                {
                    // Extract the errors
                    string[] errors = result.Errors.Select(x => x.Description).ToArray();

                    // Log the errors
                    _logger.LogError(JsonSerializer.Serialize(errors));

                    // Return the response
                    return Problem(title: "User Creation Failed",
                        statusCode: StatusCodes.Status400BadRequest, detail: JsonSerializer.Serialize(errors));
                }

                // Fetch the user
                var user = await _userManager.FindByEmailAsync(model.Email);

                // Create claims for the user
                var claimsResult = await _userManager.AddClaimsAsync(user, new List<Claim>
                {
                    new Claim(JwtClaimTypes.Id, user.Id),
                    new Claim(JwtClaimTypes.GivenName, user.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, user.LastName),
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.Scope, model.Scope)
                });

                // If claims creation failed
                if (!claimsResult.Succeeded)
                {
                    // Roll back user creation
                    await _userManager.DeleteAsync(user);

                    // Return error response
                    return Problem(title: "User Creation Failed",
                        statusCode: StatusCodes.Status500InternalServerError, detail: JsonSerializer.Serialize(claimsResult.Errors));
                }

                return Created(string.Empty, new { userId = user.Id });
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR",
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }

        /// <summary>
        /// Handles modification of a specified user
        /// </summary>
        /// <param name="userCredentials">The specified user credentials</param>
        /// <returns>The <see cref="ActionResult"/> for this transaction</returns>
        [HttpPut(EndpointRoutes.UpdateUser)]
        public async Task<ActionResult> UpdateUserAsync([FromBody] UserCredentials userCredentials, string id)
        {
            try
            {
                // Fetch the user
                var user = await _userManager.FindByIdAsync(id);

                // If user was not found...
                if (user == null)
                {
                    // Log warning
                    _logger.LogWarning("User not found");

                    // Return error response
                    return Problem(title: "NOT FOUND",
                        detail: "There is no existing user that corresponds with the specified id",
                        statusCode: StatusCodes.Status404NotFound);
                }

                // Modify the user credentials
                user.FirstName = userCredentials.FirstName ?? user.FirstName;
                user.LastName = userCredentials.LastName ?? user.LastName;
                user.PhoneNumber = userCredentials.Phone ?? user.PhoneNumber;

                // Update the date modified
                user.DateModified = DateTimeOffset.Now;

                // Update the user
                var result = await _userManager.UpdateAsync(user);

                // If user update failed...
                if (!result.Succeeded)
                {
                    // Log error
                    _logger.LogError(JsonSerializer.Serialize(result.Errors));

                    // Return error response
                    return Problem(title: "User Creation Failed",
                        detail: $"Failed to create user due to some errors\nDetail: {JsonSerializer.Serialize(result.Errors)}");
                }

                // Return response
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the issue
                _logger.LogError(ex.Message);

                // Return error response
                return Problem(title: "SYSTEM ERROR",
                    detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Generates unique account id recursively
        /// </summary>
        /// <returns>Unique account id</returns>
        private async Task<string> GenerateAccountId()
        {
            // Generate ama client ref
            var accountId = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[^0-9A-Za-z]", "")[..6].ToUpper();

            // If ama client ref exist...
            if (await _context.Users.AnyAsync(offer => offer.AccountId == accountId))
            {
                return await GenerateAccountId();
            }

            // Return the ama client ref
            return accountId;
        }
    }
}
