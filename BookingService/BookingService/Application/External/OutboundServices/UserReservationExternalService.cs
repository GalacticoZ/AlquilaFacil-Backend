using BookingService.Interfaces.ACL.Facades;

namespace BookingService.Application.External.OutboundServices;

public class UserReservationExternalService(IIamContextFacade iamContextFacade) : IUserReservationExternalService
{
    public Task<bool> UserExists(int userId)
    {
        return iamContextFacade.UserExists(userId);
    }
}