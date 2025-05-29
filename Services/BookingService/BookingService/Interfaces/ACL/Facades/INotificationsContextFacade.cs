namespace BookingService.Interfaces.ACL.Facades;

public interface INotificationsContextFacade
{
    public Task<int> CreateNotification(
        string title,
        string description,
        int userId
    );
}