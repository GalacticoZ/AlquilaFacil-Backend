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
/// Controller for notification management
/// </summary>
[Produces(MediaTypeNames.Application.Json)]
[ApiController]
[Route("api/v1/[controller]")]
public class NotificationController(INotificationCommandService notificationCommandService, INotificationQueryService notificationQueryService) : ControllerBase
{
    /// <summary>
    /// GET endpoint to retrieve all notifications for a specific user
    /// </summary>
    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(NotificationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNotificationsByUserId(int userId)
    {
        try
        {
            var query = new GetNotificationsByUserIdQuery(userId);
            var notifications = await notificationQueryService.Handle(query);
            var notificationResources = notifications.Select(NotificationResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(notificationResources);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// DELETE endpoint to remove a specific notification
    /// </summary>
    [HttpDelete("{notificationId}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteNotification(int notificationId)
    {
        try
        {
            var command = new DeleteNotificationCommand(notificationId);
            var notification = await notificationCommandService.Handle(command);
            if (notification is null) return BadRequest(new { Error = "Failed to delete notification" });
            return StatusCode(200, notification);
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
}