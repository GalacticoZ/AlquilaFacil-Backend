using NotificationService.Domain.Models.Aggregates;
using NotificationService.Domain.Models.Commands;
using NotificationService.Domain.Repositories;
using NotificationService.Domain.Services;
using NotificationService.Shared.Domain.Repositories;

namespace NotificationService.Application.Internal.CommandServices;

public class NotificationCommandService(IUnitOfWork unitOfWork, INotificationRepository notificationRepository) : INotificationCommandService
{
    public async Task<Notification> Handle(CreateNotificationCommand command)
    {
        var notification = new Notification(command);
        await notificationRepository.AddAsync(notification);
        await unitOfWork.CompleteAsync();
        return notification;
    }

    public async Task<Notification> Handle(DeleteNotificationCommand command)
    {
        var notification = await notificationRepository.FindByIdAsync(command.Id);
        if (notification == null)
        {
            throw new Exception("Notification not found");
        }
        notificationRepository.Remove(notification);
        await unitOfWork.CompleteAsync();
        return notification;
    }
}