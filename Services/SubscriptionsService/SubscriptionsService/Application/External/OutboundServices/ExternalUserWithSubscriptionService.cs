using SubscriptionsService.Interfaces.ACL.Facades;

namespace SubscriptionsService.Application.External.OutboundServices;

public class ExternalUserWithSubscriptionService(IIamContextFacade iamContextFacade) : IExternalUserWithSubscriptionService
{
    public Task<bool> UserExists(int id)
    {
        return iamContextFacade.UserExists(id);
    }
}