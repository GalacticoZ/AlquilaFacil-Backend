using Shared.Interfaces.ACL.DTOs;

namespace Shared.Interfaces.ACL.Facades;

public interface ISubscriptionContextFacade
{
    Task<IEnumerable<SubscriptionDTO>> GetSubscriptionByUserIdsList(List<int> userIdsList);
}