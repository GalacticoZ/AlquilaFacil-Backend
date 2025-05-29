using BookingService.Interfaces.ACL.DTOs;

namespace BookingService.Interfaces.ACL.Facades;

public interface ISubscriptionContextFacade
{
    Task<IEnumerable<SubscriptionDTO>> GetSubscriptionByUserIdsList(List<int> userIdsList);
}