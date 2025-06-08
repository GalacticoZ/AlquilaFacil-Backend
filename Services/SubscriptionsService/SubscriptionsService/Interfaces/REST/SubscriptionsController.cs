using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ProfilesService.Domain.Model.Queries;
using SubscriptionsService.Domain.Model.Commands;
using SubscriptionsService.Domain.Model.Queries;
using SubscriptionsService.Domain.Services;
using SubscriptionsService.Interfaces.REST.Resources;
using SubscriptionsService.Interfaces.REST.Transform;
using Shared.Interfaces.REST.Resources;

namespace SubscriptionsService.Interfaces.REST;

/// <summary>
/// Controlador para gestión de suscripciones
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class SubscriptionsController(
    ISubscriptionCommandService subscriptionCommandService,
    ISubscriptionQueryServices subscriptionQueryServices)
    : ControllerBase
{
    /// <summary>
    /// Endpoint POST para crear una nueva suscripción
    /// </summary>
    /// <param name="createSubscriptionResource">Datos de la suscripción a crear</param>
    /// <returns>Suscripción creada</returns>
    /// <response code="201">Suscripción creada exitosamente</response>
    /// <response code="400">Datos de entrada inválidos o no se pudo crear la suscripción</response>
    /// <response code="404">Recurso relacionado no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateSubscription(
        [FromBody] CreateSubscriptionResource createSubscriptionResource)
    {
        try
        {
            // Convierte el recurso de entrada en comando de creación
            var createSubscriptionCommand =
                CreateSubscriptionCommandFromResourceAssembler.ToCommandFromResource(createSubscriptionResource);
            // Ejecuta el comando para crear la suscripción
            var subscription = await subscriptionCommandService.Handle(createSubscriptionCommand);
            // Valida que la suscripción se creó correctamente
            if (subscription is null) return BadRequest(new { Error = "Failed to create subscription" });
            // Convierte la entidad creada en recurso de respuesta
            var resource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription);
            return StatusCode(201, resource);
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
    /// Endpoint GET para obtener todas las suscripciones disponibles
    /// </summary>
    /// <returns>Lista de todas las suscripciones</returns>
    /// <response code="200">Suscripciones obtenidas exitosamente</response>
    /// <response code="404">No se encontraron suscripciones o error en la consulta</response>
    [HttpGet]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllSubscriptions()
    {
        try
        {
            // Crea la query para obtener todas las suscripciones
            var getAllSubscriptionsQuery = new GetAllSubscriptionsQuery();
            // Ejecuta la consulta para obtener las suscripciones
            var subscriptions = await subscriptionQueryServices.Handle(getAllSubscriptionsQuery);
            // Convierte cada entidad suscripción a recurso de respuesta
            var resources = subscriptions.Select(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentran suscripciones o falla la consulta
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// Endpoint GET para obtener el estado de suscripción de un usuario específico
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <returns>Estado de la suscripción del usuario</returns>
    /// <response code="200">Estado de suscripción obtenido exitosamente</response>
    /// <response code="404">Usuario no encontrado o sin suscripción</response>
    [HttpGet("status/{userId}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubscriptionStatusByUserId([FromRoute] int userId)
    {
        try
        {
            // Crea la query con el ID del usuario
            var getSubscriptionStatusByUserIdQuery = new GetSubscriptionStatusByUserIdQuery(userId);
            // Ejecuta la consulta para obtener el estado de suscripción
            var subscriptionStatus = await subscriptionQueryServices.Handle(getSubscriptionStatusByUserIdQuery);
            return Ok(subscriptionStatus);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentra el estado de suscripción o falla la consulta
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// Endpoint PUT para activar el estado de una suscripción
    /// </summary>
    /// <param name="subscriptionId">ID de la suscripción a activar</param>
    /// <returns>Suscripción con estado actualizado</returns>
    /// <response code="200">Suscripción activada exitosamente</response>
    /// <response code="400">Datos de entrada inválidos</response>
    /// <response code="404">Suscripción no encontrada</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPut("{subscriptionId}")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActiveSubscriptionStatus(int subscriptionId)
    {
        try
        {
            // Crea el comando con el ID de la suscripción a activar
            var activeSubscriptionStatusCommand = new ActiveSubscriptionStatusCommand(subscriptionId);
            // Ejecuta el comando para activar la suscripción
            var subscription = await subscriptionCommandService.Handle(activeSubscriptionStatusCommand);
            // Valida que la suscripción se activó correctamente
            if (subscription == null) return NotFound(new { Error = "Subscription not found" });
            // Convierte la entidad actualizada en recurso de respuesta
            var resource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription);
            return Ok(resource);
        }
        catch (BadHttpRequestException ex)
        {
            // Maneja errores de validación de datos de entrada
            return BadRequest(new { Error = ex.Message }); // 400
        }
        catch (KeyNotFoundException ex)
        {
            // Maneja casos donde no se encuentra la suscripción
            return NotFound(new { Error = ex.Message }); // 404
        }
        catch (Exception ex)
        {
            // Maneja errores generales del sistema
            return StatusCode(500, new { Error = "Internal server error", Details = ex.Message }); // 500
        }
    }
}