using Microsoft.OpenApi.Extensions;
using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Interfaces.REST.Resources;

namespace SubscriptionsService.Interfaces.REST.Transform;

public static class SubscriptionResourceFromEntityAssembler
{
    public static SubscriptionResource ToResourceFromEntity(Subscription entity)
    {
        return new SubscriptionResource(entity.Id, entity.UserId, entity.PlanId,
            entity.SubscriptionStatusId, entity.VoucherImageUrl);
    }
}