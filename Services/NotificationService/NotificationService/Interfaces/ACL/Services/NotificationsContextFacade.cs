using NotificationService.Domain.Models.Commands;
using NotificationService.Domain.Services;

namespace NotificationService.Interfaces.ACL.Services;

public class NotificationsContextFacade(INotificationCommandService notificationCommandService) : INotificationsContextFacade
{
    public async Task<int> CreateNotification(
        string title,
        string description,
        int userId
    )
    {
        var createNotificationCommand = new CreateNotificationCommand(title, description, userId);
        var notification = await notificationCommandService.Handle(createNotificationCommand);
        return notification?.Id ?? 0;
    }
}