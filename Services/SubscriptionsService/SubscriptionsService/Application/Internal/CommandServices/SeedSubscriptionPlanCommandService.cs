using Shared.Domain.Repositories;
using SubscriptionsService.Domain.Model.Commands;
using SubscriptionsService.Domain.Repositories;
using SubscriptionsService.Domain.Services;

namespace SubscriptionsService.Application.Internal.CommandServices;

public class SeedSubscriptionPlanCommandService(IPlanRepository repository, IPlanCommandService commandService, IUnitOfWork unitOfWork): ISeedSubscriptionPlanCommandService
{
    public async Task Handle(SeedSubscriptionPlanCommand command)
    {
        var existingPlan = await repository.FindByIdAsync(1);
        if (existingPlan != null) return;
        var planCommand = new CreatePlanCommand("Plan Premium",
            "El plan premium te permitirá acceder a funcionalidades adicionales en la aplicación", 20);
        await commandService.Handle(planCommand);
        await unitOfWork.CompleteAsync();
    }
}
