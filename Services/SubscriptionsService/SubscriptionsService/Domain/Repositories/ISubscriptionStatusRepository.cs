using Shared.Domain.Repositories;
using SubscriptionsService.Domain.Model.Entities;
using SubscriptionsService.Domain.Model.ValueObjects;

namespace SubscriptionsService.Domain.Repositories;

public interface ISubscriptionStatusRepository : IBaseRepository<SubscriptionStatus>
{
    Task<bool> ExistsBySubscriptionStatus(ESubscriptionStatus subscriptionStatus);
}