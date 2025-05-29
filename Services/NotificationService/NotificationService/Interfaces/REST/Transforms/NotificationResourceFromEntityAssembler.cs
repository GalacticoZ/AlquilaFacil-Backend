using NotificationService.Domain.Models.Aggregates;
using NotificationService.Interfaces.REST.Resources;

namespace NotificationService.Interfaces.REST.Transforms;

public static class NotificationResourceFromEntityAssembler
{
    public static NotificationResource ToResourceFromEntity(Notification notification)
    {
        return new NotificationResource
        (
            notification.Id,
            notification.Title,
            notification.Description,
            notification.UserId
        );
    }
}