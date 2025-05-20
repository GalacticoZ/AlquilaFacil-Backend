using ProfilesService.Domain.Model.Queries;
using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Domain.Model.Queries;

namespace SubscriptionsService.Domain.Services;

public interface ISubscriptionQueryServices
{
    Task<Subscription?> Handle(GetSubscriptionByIdQuery query);
    Task<IEnumerable<Subscription>> Handle(GetAllSubscriptionsQuery query);
    Task<string> Handle(GetSubscriptionStatusByUserIdQuery query);
    Task<IEnumerable<Subscription>> Handle(GetSubscriptionsByUserIdQuery query);
    
}