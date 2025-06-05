using Shared.Domain.Repositories;
using SubscriptionsService.Application.External.OutboundServices;
using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Domain.Model.Commands;
using SubscriptionsService.Domain.Repositories;
using SubscriptionsService.Domain.Services;

namespace SubscriptionsService.Application.Internal.CommandServices;

public class SubscriptionCommandService(ISubscriptionRepository subscriptionRepository, ISubscriptionStatusRepository subscriptionStatusRepository,
    IPlanRepository planRepository, 
    IUnitOfWork unitOfWork, IExternalUserWithSubscriptionService externalUserWithSubscriptionService)
    : ISubscriptionCommandService
{
    public async Task<Subscription?> Handle(CreateSubscriptionCommand command)
    {
        var subscription = new Subscription(command);
        var plan = await planRepository.FindByIdAsync(command.PlanId);
        if (plan == null)
        {
            throw new Exception("Plan not found");
        }
        
        var userExists = await externalUserWithSubscriptionService.UserExists(command.UserId);

        if (!userExists)
        {
            throw new Exception("User not found");
        }
        
        await subscriptionRepository.AddAsync(subscription);
        await unitOfWork.CompleteAsync();
        return subscription;
    }

    public async Task<Subscription?> Handle(ActiveSubscriptionStatusCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId);
        if (subscription == null)
        {
            throw new Exception("Subscription not found");
        }
        subscription.ActiveSubscriptionStatus();
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        return subscription;
    }
}