using SubscriptionsService.Shared.Domain.Repositories;
using SubscriptionsService.Domain.Model.Aggregates;

namespace SubscriptionsService.Domain.Repositories;

public interface ISubscriptionRepository : IBaseRepository<Subscription>
{
    Task<Subscription?> FindByUserIdAsync(int userId);
    Task<IEnumerable<Subscription>> FindByUserIdsListAsync(List<int> userIdsList);
}