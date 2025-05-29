using SubscriptionsService.Domain.Model.Entities;
using SubscriptionsService.Shared.Domain.Repositories;
using SubscriptionsService.Domain.Model.Aggregates;

namespace SubscriptionsService.Domain.Repositories;

public interface IPlanRepository : IBaseRepository<Plan>
{
}