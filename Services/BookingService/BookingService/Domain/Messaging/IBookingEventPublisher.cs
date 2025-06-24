using Shared.Domain.Model.Events;

namespace BookingService.Domain.Messaging;

public interface IBookingEventPublisher
{
    public Task PublishAsync(BookingCreatedEvent bookingCreatedEvent);
    
}