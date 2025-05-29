using NotificationService.Domain.Models.Commands;
using NotificationService.Interfaces.REST.Resources;

namespace NotificationService.Notifications.Interfaces.REST.Transforms;

public static class CreateNotificationCommandFromResourceAssembler
{
    public static CreateNotificationCommand ToCommandFromResource(CreateNotificationResource createNotificationResource)
    {
        return new CreateNotificationCommand(
            createNotificationResource.Title,
            createNotificationResource.Description,
            createNotificationResource.UserId
            );
    }
}