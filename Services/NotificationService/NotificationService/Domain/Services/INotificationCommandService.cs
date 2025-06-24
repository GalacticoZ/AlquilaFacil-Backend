using NotificationService.Domain.Models.Aggregates;
using NotificationService.Domain.Models.Commands;

namespace NotificationService.Domain.Services;

public interface INotificationCommandService
{
    Task<Notification> Handle(CreateNotificationCommand command);
    Task<Notification> Handle(DeleteNotificationCommand command);
}