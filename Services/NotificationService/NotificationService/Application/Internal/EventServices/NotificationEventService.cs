using Microsoft.AspNetCore.Components.Web;
using NotificationService.Domain.Models.Aggregates;
using NotificationService.Domain.Repositories;
using NotificationService.Domain.Services;
using NotificationService.Infrastructure.Persistence.EFC.Repositories;
using Shared.Domain.Model.Events;
using Shared.Domain.Repositories;

namespace NotificationService.Application.Internal.EventServices;

public class NotificationEventService(ILogger<NotificationEventService> logger, INotificationRepository notificationRepository, IUnitOfWork unitOfWork) : INotificationEventService
{
    public async Task<Notification> Handle(BookingCreatedEvent bookingCreatedEvent)
    {
        var notification = new Notification
        ("Reserva creada" + bookingCreatedEvent.CreatedAt, bookingCreatedEvent.Content, bookingCreatedEvent.OwnerId);
        await notificationRepository.AddAsync(notification);
        await unitOfWork.CompleteAsync();
        return notification;
    }
}