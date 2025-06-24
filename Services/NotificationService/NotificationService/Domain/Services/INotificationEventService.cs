using NotificationService.Domain.Models.Aggregates;
using Shared.Domain.Model.Events;

namespace NotificationService.Domain.Services;

public interface INotificationEventService
{
    public Task<Notification> Handle(BookingCreatedEvent bookingCreatedEvent);
}