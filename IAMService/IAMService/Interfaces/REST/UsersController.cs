using System.Net.Mime;
using IAMService.Application.Internal.CommandServices;
using IAMService.Domain.Model.Aggregates;
using IAMService.Domain.Model.Commands;
using IAMService.Domain.Model.Queries;
using IAMService.Domain.Model.ValueObjects;
using IAMService.Domain.Services;
using IAMService.Infrastructure.Pipeline.Middleware.Attributes;
using IAMService.Interfaces.ACL.Facades;
using IAMService.Interfaces.REST.Resources;
using IAMService.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace IAMService.Interfaces.REST;

/**
 * <summary>
 *     The users controller
 * </summary>
 * <remarks>
 *     This class is used to handle user requests
 * </remarks>
 */

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class UsersController(
    IIamContextFacade iamContextFacade, IUserQueryService userQueryService, IUserCommandService userCommandService
    ) : ControllerBase
{
    
    
    /**
     * <summary>
     *     Get user by id endpoint. It allows to get a user by id
     * </summary>
     * <param name="userId">The user id</param>
     * <returns>The user resource</returns>
     */
    //[AuthorizeRole(EUserRoles.Admin)]
    [HttpGet("{userId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var user = await userQueryService.Handle(getUserByIdQuery);
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user!);
        return Ok(userResource);
    }
    
    
    /**
     * <summary>
     *     Get all users endpoint. It allows to get all users
     * </summary>
     * <returns>The user resources</returns>
     */
    [HttpGet]
    //[AuthorizeRole(EUserRoles.Admin)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllUsers()
    {
        var getAllUsersQuery = new GetAllUsersQuery();
        var users = await userQueryService.Handle(getAllUsersQuery);
        var userResources = users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(userResources);
    }
    
    [HttpGet("get-username/{userId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUsernameById(int userId)
    {
        var getUsernameByIdQuery = new GetUsernameByIdQuery(userId);
        var username = await userQueryService.Handle(getUsernameByIdQuery);
        return Ok(username);
    }
    
    [HttpPut("{userId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUsernameResource updateUsernameResource)
    {
        var updateUserCommand =
            UpdateUsernameCommandFromResourceAssembler.ToUpdateUsernameCommand(userId, updateUsernameResource);
        var user = await userCommandService.Handle(updateUserCommand);
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user!);
        return Ok(userResource);
    }
    
    [HttpGet("user-exists/{userId:int}")]
    [AllowAnonymous]
    public IActionResult IsUserExists([FromRoute] int userId)
    {
        try {
            var isUsersExists = iamContextFacade.UsersExists(userId);
            return Ok(isUsersExists);
        }catch(Exception e) {
            return BadRequest(new { message = e.Message });
        }

    }
}