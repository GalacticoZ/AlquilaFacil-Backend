namespace BookingService.Application.External.OutboundServices;

public interface INotificationReservationExternalService
{
    Task<int> CreateNotification(
        string title,
        string description,
        int userId
    );
}