using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Domain.Model.Commands;

namespace SubscriptionsService.Domain.Services;

public interface ISubscriptionCommandService
{
    public Task<Subscription?> Handle(CreateSubscriptionCommand command);

    public Task<Subscription?> Handle(ActiveSubscriptionStatusCommand command);
}