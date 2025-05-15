namespace BookingService.Application.External.OutboundServices;

public interface IUserReservationExternalService
{
    Task<bool> UserExists(int userId);
}