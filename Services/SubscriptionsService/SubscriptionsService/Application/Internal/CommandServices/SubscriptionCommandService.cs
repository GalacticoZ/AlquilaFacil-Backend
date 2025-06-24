using Shared.Application.External.OutboundServices;
using Shared.Domain.Repositories;
using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Domain.Model.Commands;
using SubscriptionsService.Domain.Repositories;
using SubscriptionsService.Domain.Services;

namespace SubscriptionsService.Application.Internal.CommandServices;

public class SubscriptionCommandService(ISubscriptionRepository subscriptionRepository, ISubscriptionStatusRepository subscriptionStatusRepository,
    IPlanRepository planRepository, 
    IUnitOfWork unitOfWork, IUserExternalService userExternalService)
    : ISubscriptionCommandService
{
    public async Task<Subscription?> Handle(CreateSubscriptionCommand command)
    {
        var subscription = new Subscription(command);
        var plan = await planRepository.FindByIdAsync(command.PlanId);
        if (plan == null)
        {
            throw new KeyNotFoundException("Plan not found");
        }
        
        var userExists = await userExternalService.UserExists(command.UserId);

        if (!userExists)
        {
            throw new KeyNotFoundException("User not found");
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
            throw new KeyNotFoundException("Subscription not found");
        }
        subscription.ActiveSubscriptionStatus();
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        return subscription;
    }
}