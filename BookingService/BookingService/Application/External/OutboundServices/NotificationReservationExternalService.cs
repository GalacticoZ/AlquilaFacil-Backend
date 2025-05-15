using BookingService.Application.External.OutboundServices;
using BookingService.Interfaces.ACL.Facades;

namespace BookingService.Application.External.OutboundServices;

public class NotificationReservationExternalService(INotificationsContextFacade notificationsContextFacade) : INotificationReservationExternalService
{
    public async Task<int> CreateNotification(
        string title,
        string description,
        int userId
    )
    {
        return await notificationsContextFacade.CreateNotification(title, description, userId);
    }
}