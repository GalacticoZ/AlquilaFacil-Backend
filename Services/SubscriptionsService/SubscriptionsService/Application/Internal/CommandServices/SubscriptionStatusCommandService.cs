using SubscriptionsService.Shared.Domain.Repositories;
using SubscriptionsService.Domain.Model.Commands;
using SubscriptionsService.Domain.Model.Entities;
using SubscriptionsService.Domain.Model.ValueObjects;
using SubscriptionsService.Domain.Repositories;
using SubscriptionsService.Domain.Services;

namespace SubscriptionsService.Subscriptions.Application.Internal.CommandServices;

public class SubscriptionStatusCommandService(ISubscriptionStatusRepository subscriptionStatusRepository, IUnitOfWork unitOfWork) : ISubscriptionStatusCommandService
{
    public async Task Handle(SeedSubscriptionStatusCommand command)
    {
        foreach (ESubscriptionStatus status in Enum.GetValues(typeof(ESubscriptionStatus)))
        {
            if (await subscriptionStatusRepository.ExistsBySubscriptionStatus(status)) continue;
            var subscriptionStatus = new SubscriptionStatus(status.ToString());
            await subscriptionStatusRepository.AddAsync(subscriptionStatus);
        }

        await unitOfWork.CompleteAsync();
    }

}