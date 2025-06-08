using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ProfilesService.Domain.Model.Aggregates;
using ProfilesService.Domain.Model.Queries;
using ProfilesService.Domain.Services;
using ProfilesService.Interfaces.REST.Resources;
using ProfilesService.Interfaces.REST.Transform;
using Shared.Interfaces.REST.Resources;

namespace ProfilesService.Interfaces.REST;

/// <summary>
/// Controlador para gestión de perfiles de usuario
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ProfilesController(
    IProfileCommandService profileCommandService, 
    IProfileQueryService profileQueryService)
    : ControllerBase
{
    /// <summary>
    /// Endpoint POST para crear un nuevo perfil de usuario
    /// </summary>
    /// <param name="resource">Datos del perfil a crear</param>
    /// <returns>Perfil creado</returns>
    /// <response code="201">Perfil creado exitosamente</response>
    /// <response code="400">Datos de entrada inválidos o no se pudo crear el perfil</response>
    /// <response code="404">Recurso relacionado no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProfileResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProfile(CreateProfileResource resource)
    {
        try
        {
            // Convierte el recurso de entrada en comando de creación
            var createProfileCommand = CreateProfileCommandFromResourceAssembler.ToCommandFromResource(resource);
            // Ejecuta el comando para crear el perfil
            var profile = await profileCommandService.Handle(createProfileCommand);
            // Valida que el perfil se creó correctamente
            if (profile is null) return BadRequest(new { Error = "Failed to create profile" });
            // Convierte la entidad creada en recurso de respuesta
            var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
            return StatusCode(201, profileResource);
        }
        catch (BadHttpRequestException ex)
        {
            // Maneja errores de validación de datos de entrada
            return BadRequest(new { Error = ex.Message }); // 400
        }
        catch (KeyNotFoundException ex)
        {
            // Maneja casos donde no se encuentra el recurso relacionado
            return NotFound(new { Error = ex.Message }); // 404
        }
        catch (Exception ex)
        {
            // Maneja errores generales del sistema
            return StatusCode(500, new { Error = "Internal server error", Details = ex.Message }); // 500
        }
    }

    /// <summary>
    /// Endpoint GET para obtener el perfil de un usuario específico
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <returns>Perfil del usuario</returns>
    /// <response code="200">Perfil obtenido exitosamente</response>
    /// <response code="404">Perfil no encontrado</response>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(ProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileByUserId(int userId)
    {
        try
        {
            // Ejecuta la consulta para obtener el perfil del usuario
            var profile = await profileQueryService.Handle(new GetProfileByUserIdQuery(userId));
            // Valida que el perfil existe
            if (profile == null) return NotFound(new { Error = "Profile not found" });
            // Convierte la entidad perfil a recurso de respuesta
            var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
            return Ok(profileResource);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentra el perfil o falla la consulta
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// Endpoint GET para obtener las cuentas bancarias de un usuario específico
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <returns>Lista de cuentas bancarias del usuario</returns>
    /// <response code="200">Cuentas bancarias obtenidas exitosamente</response>
    /// <response code="404">Usuario no encontrado o sin cuentas bancarias</response>
    [HttpGet("bank-accounts/{userId}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileBankAccountsByUserId(int userId)
    {
        try
        {
            // Crea la query con el ID del usuario
            var query = new GetProfileBankAccountsByUserIdQuery(userId);
            // Ejecuta la consulta para obtener las cuentas bancarias del usuario
            var bankAccounts = await profileQueryService.Handle(query);
            return Ok(bankAccounts);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentran cuentas bancarias o falla la consulta
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// Endpoint PUT para actualizar un perfil de usuario existente
    /// </summary>
    /// <param name="userId">ID del usuario cuyo perfil se va a actualizar</param>
    /// <param name="updateProfileResource">Datos actualizados del perfil</param>
    /// <returns>Perfil actualizado</returns>
    /// <response code="200">Perfil actualizado exitosamente</response>
    /// <response code="400">Datos de entrada inválidos</response>
    /// <response code="404">Perfil no encontrado o no se pudo actualizar</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPut("{userId}")]
    [ProducesResponseType(typeof(ProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateProfile(int userId, [FromBody] UpdateProfileResource updateProfileResource)
    {
        try
        {
            // Convierte el recurso de entrada en comando de actualización
            var updateProfileCommand = UpdateProfileCommandFromResourceAssembler.ToCommandFromResource(userId, updateProfileResource);
            // Ejecuta el comando para actualizar el perfil
            var result = await profileCommandService.Handle(updateProfileCommand);
            // Valida que el perfil se actualizó correctamente
            if (result is null) return NotFound(new { Error = "Profile not found or failed to update" });
            // Convierte la entidad actualizada en recurso de respuesta
            return Ok(ProfileResourceFromEntityAssembler.ToResourceFromEntity(result));
        }
        catch (BadHttpRequestException ex)
        {
            // Maneja errores de validación de datos de entrada
            return BadRequest(new { Error = ex.Message }); // 400
        }
        catch (KeyNotFoundException ex)
        {
            // Maneja casos donde no se encuentra el perfil a actualizar
            return NotFound(new { Error = ex.Message }); // 404
        }
        catch (Exception ex)
        {
            // Maneja errores generales del sistema
            return StatusCode(500, new { Error = "Internal server error", Details = ex.Message }); // 500
        }
    }
}