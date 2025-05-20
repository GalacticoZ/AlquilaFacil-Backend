using SubscriptionsService.Domain.Model.Commands;
using SubscriptionsService.Interfaces.REST.Resources;

namespace SubscriptionsService.Interfaces.REST.Transform;

public static class CreateSubscriptionCommandFromResourceAssembler
{
    public static CreateSubscriptionCommand ToCommandFromResource(CreateSubscriptionResource resource)
    {
        return new CreateSubscriptionCommand(resource.UserId, resource.PlanId, resource.VoucherImageUrl);
    }
}