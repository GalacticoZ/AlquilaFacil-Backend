using ProfilesService.Domain.Model.Queries;
using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Domain.Model.Queries;
using SubscriptionsService.Domain.Model.ValueObjects;
using SubscriptionsService.Domain.Repositories;
using SubscriptionsService.Domain.Services;

namespace SubscriptionsService.Application.Internal.QueryServices;

public class SubscriptionQueryService(ISubscriptionRepository subscriptionRepository) : ISubscriptionQueryServices
{
    public async Task<Subscription?> Handle(GetSubscriptionByIdQuery query)
    {
        return await subscriptionRepository.FindByIdAsync(query.Id);
    }
    
    public async Task<IEnumerable<Subscription>> Handle(GetAllSubscriptionsQuery query)
    {
        return await subscriptionRepository.ListAsync();
    }

    public async Task<string> Handle(GetSubscriptionStatusByUserIdQuery query)
    {
        var subscription = await subscriptionRepository.FindByUserIdAsync(query.UserId);
        if (subscription == null)
        {
            return "No subscription found";
        }
        return ((ESubscriptionStatus)subscription.SubscriptionStatusId).ToString();
    }

    public async Task<IEnumerable<Subscription>> Handle(GetSubscriptionsByUserIdQuery query)
    {
        return await subscriptionRepository.FindByUserIdsListAsync(query.UserIds);
    }
    
}