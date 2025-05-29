
namespace SubscriptionsService.Application.External.OutboundServices;

public interface IExternalUserWithSubscriptionService
{
    Task<bool> UserExists(int userId);
}