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
/// This class is used to handle user requests
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
    /// Get user by id endpoint. It allows to get a user by id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <returns>The user resource</returns>
    /// <response code="200">Usuario obtenido exitosamente</response>
    /// <response code="404">Usuario no encontrado</response>
    //[AuthorizeRole(EUserRoles.Admin)]
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
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
    
    /// <summary>
    /// Get all users endpoint. It allows to get all users
    /// </summary>
    /// <returns>The user resources</returns>
    /// <response code="200">Usuarios obtenidos exitosamente</response>
    /// <response code="404">No se encontraron usuarios</response>
    [HttpGet]
    //[AuthorizeRole(EUserRoles.Admin)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
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
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
    
    /// <summary>
    /// Endpoint GET para obtener nombre de usuario por ID
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <returns>Nombre del usuario</returns>
    /// <response code="200">Nombre de usuario obtenido exitosamente</response>
    /// <response code="404">Usuario no encontrado</response>
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
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
    
    /// <summary>
    /// Endpoint PUT para actualizar un usuario
    /// </summary>
    /// <param name="userId">ID del usuario a actualizar</param>
    /// <param name="updateUsernameResource">Datos actualizados del usuario</param>
    /// <returns>Usuario actualizado</returns>
    /// <response code="200">Usuario actualizado exitosamente</response>
    /// <response code="400">Datos de entrada inválidos</response>
    /// <response code="404">Usuario no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
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
            return BadRequest(new { Error = ex.Message }); // 400
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Internal server error", Details = ex.Message }); // 500
        }
    }
    
    /// <summary>
    /// Endpoint GET para verificar si un usuario existe
    /// </summary>
    /// <param name="userId">ID del usuario a verificar</param>
    /// <returns>Verdadero si el usuario existe, falso en caso contrario</returns>
    /// <response code="200">Verificación exitosa</response>
    /// <response code="404">Usuario no existe</response>
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
            if (exists) {
                return Ok(exists);
            }
            return NotFound(new { message = "User does not exist." });
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
}