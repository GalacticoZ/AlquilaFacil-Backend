using SubscriptionsService.Domain.Model.Commands;

namespace SubscriptionsService.Domain.Services;

public interface ISubscriptionStatusCommandService
{
    Task Handle(SeedSubscriptionStatusCommand command);
}