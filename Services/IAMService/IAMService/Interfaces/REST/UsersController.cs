using System.Net.Mime;
using IAMService.Domain.Model.Queries;
using IAMService.Domain.Services;
using IAMService.Infrastructure.Pipeline.Middleware.Attributes;
using IAMService.Interfaces.REST.Resources;
using IAMService.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.REST.Resources;

namespace IAMService.Interfaces.REST;

/// <summary>
/// The users controller
/// </summary>
/// <remarks>
/// This class handles user-related HTTP requests.
/// </remarks>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class UsersController(
    IUserQueryService userQueryService, IUserCommandService userCommandService
    ) : ControllerBase
{
    /// <summary>
    /// Get user by ID.
    /// </summary>
    /// <param name="userId">The user's ID</param>
    /// <returns>The user resource</returns>
    /// <response code="200">User successfully retrieved</response>
    /// <response code="404">User not found</response>
    [HttpGet("{userId:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(int userId)
    {
        try
        {
            var getUserByIdQuery = new GetUserByIdQuery(userId);
            var user = await userQueryService.Handle(getUserByIdQuery);
            if (user is null) return NotFound(new { Error = "User not found" });
            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
            return Ok(userResource);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// Get all users.
    /// </summary>
    /// <returns>List of user resources</returns>
    /// <response code="200">Users successfully retrieved</response>
    /// <response code="404">No users found</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<UserResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var getAllUsersQuery = new GetAllUsersQuery();
            var users = await userQueryService.Handle(getAllUsersQuery);
            var userResources = users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(userResources);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// Get a username by user ID.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>Username</returns>
    /// <response code="200">Username successfully retrieved</response>
    /// <response code="404">User not found</response>
    [HttpGet("get-username/{userId:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUsernameById(int userId)
    {
        try
        {
            var getUsernameByIdQuery = new GetUsernameByIdQuery(userId);
            var username = await userQueryService.Handle(getUsernameByIdQuery);
            return Ok(username);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// Update a user by ID.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="updateUsernameResource">Updated user data</param>
    /// <returns>Updated user resource</returns>
    /// <response code="200">User successfully updated</response>
    /// <response code="400">Invalid input data</response>
    /// <response code="404">User not found</response>
    /// <response code="500">Internal server error</response>
    [HttpPut("{userId:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUsernameResource updateUsernameResource)
    {
        try
        {
            var updateUserCommand =
                UpdateUsernameCommandFromResourceAssembler.ToUpdateUsernameCommand(userId, updateUsernameResource);
            var user = await userCommandService.Handle(updateUserCommand);
            if (user is null) return NotFound(new { Error = "User not found or failed to update" });
            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
            return Ok(userResource);
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Internal server error", Details = ex.Message });
        }
    }

    /// <summary>
    /// Check if a user exists by ID.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>True if user exists, false otherwise</returns>
    /// <response code="200">Check completed successfully</response>
    /// <response code="404">User does not exist</response>
    [HttpGet("user-exists/{userId:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> IsUserExists([FromRoute] int userId)
    {
        try
        {
            var userExistsQuery = new UserExistsQuery(userId);
            var exists =  userQueryService.Handle(userExistsQuery);
            if (exists)
            {
                return Ok(exists);
            }
            return NotFound(new { message = "User does not exist." });
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message });
        }
    }
}
