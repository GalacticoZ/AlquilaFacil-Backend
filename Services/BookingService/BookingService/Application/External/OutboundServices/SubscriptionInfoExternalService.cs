using Shared.Interfaces.ACL.DTOs;
using Shared.Interfaces.ACL.Facades;

namespace BookingService.Application.External.OutboundServices;


public class SubscriptionInfoExternalService(ISubscriptionContextFacade subscriptionContextFacade) : ISubscriptionInfoExternalService
{
    public async Task<IEnumerable<SubscriptionDTO>> GetSubscriptionByUserIdsList(List<int> userIdsList)
    {
        return await subscriptionContextFacade.GetSubscriptionByUserIdsList(userIdsList);
    }
}