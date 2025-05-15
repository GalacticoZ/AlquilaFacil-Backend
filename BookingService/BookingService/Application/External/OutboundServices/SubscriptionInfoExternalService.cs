using BookingService.Interfaces.ACL.DTOs;
using BookingService.Interfaces.ACL.Facades;

namespace BookingService.Application.External.OutboundServices;


public class SubscriptionInfoExternalService(ISubscriptionContextFacade subscriptionContextFacade) : ISubscriptionInfoExternalService
{
    public async Task<IEnumerable<SubscriptionDTO>> GetSubscriptionByUserIdsList(List<int> userIdsList)
    {
        return await subscriptionContextFacade.GetSubscriptionByUserIdsList(userIdsList);
    }
}