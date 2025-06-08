using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Domain.Models.Commands;
using NotificationService.Domain.Models.Queries;
using NotificationService.Domain.Services;
using NotificationService.Interfaces.REST.Resources;
using NotificationService.Interfaces.REST.Transforms;
using Shared.Interfaces.REST.Resources;

namespace NotificationService.Interfaces.REST;

/// <summary>
/// Controlador para gestión de notificaciones
/// </summary>
[Produces(MediaTypeNames.Application.Json)]
[ApiController]
[Route("api/v1/[controller]")]
public class NotificationController(INotificationCommandService notificationCommandService, INotificationQueryService notificationQueryService) : ControllerBase
{
    /// <summary>
    /// Endpoint GET para obtener todas las notificaciones de un usuario específico
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <returns>Lista de notificaciones del usuario</returns>
    /// <response code="200">Notificaciones obtenidas exitosamente</response>
    /// <response code="404">Usuario no encontrado o sin notificaciones</response>
    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(NotificationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNotificationsByUserId(int userId)
    {
        try
        {
            // Crea la query con el ID del usuario
            var query = new GetNotificationsByUserIdQuery(userId);
            // Ejecuta la consulta para obtener las notificaciones del usuario
            var notifications = await notificationQueryService.Handle(query);
            // Convierte cada entidad notificación a recurso de respuesta
            var notificationResources = notifications.Select(NotificationResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(notificationResources);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentran notificaciones del usuario o falla la consulta
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// Endpoint DELETE para eliminar una notificación específica
    /// </summary>
    /// <param name="notificationId">ID de la notificación a eliminar</param>
    /// <returns>Confirmación de eliminación</returns>
    /// <response code="200">Notificación eliminada exitosamente</response>
    /// <response code="400">No se pudo eliminar la notificación</response>
    /// <response code="404">Notificación no encontrada</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpDelete("{notificationId}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteNotification(int notificationId)
    {
        try
        {
            // Crea el comando con el ID de la notificación a eliminar
            var command = new DeleteNotificationCommand(notificationId);
            // Ejecuta el comando para eliminar la notificación
            var notification = await notificationCommandService.Handle(command);
            // Valida que la notificación se eliminó correctamente
            if (notification is null) return BadRequest(new { Error = "Failed to delete notification" });
            return StatusCode(200, notification);
        }
        catch (BadHttpRequestException ex)
        {
            // Maneja errores de validación de datos de entrada
            return BadRequest(new { Error = ex.Message }); // 400
        }
        catch (KeyNotFoundException ex)
        {
            // Maneja casos donde no se encuentra la notificación
            return NotFound(new { Error = ex.Message }); // 404
        }
        catch (Exception ex)
        {
            // Maneja errores generales del sistema
            return StatusCode(500, new { Error = "Internal server error", Details = ex.Message }); // 500
        }
    }
}