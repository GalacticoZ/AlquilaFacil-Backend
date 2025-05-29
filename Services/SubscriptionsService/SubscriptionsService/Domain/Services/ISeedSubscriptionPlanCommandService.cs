using SubscriptionsService.Domain.Model.Commands;

namespace SubscriptionsService.Domain.Services;

public interface ISeedSubscriptionPlanCommandService
{
    Task Handle(SeedSubscriptionPlanCommand command);
}