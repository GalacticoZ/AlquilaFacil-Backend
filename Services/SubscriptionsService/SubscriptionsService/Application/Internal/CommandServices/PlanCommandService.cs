using Shared.Domain.Repositories;
using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Domain.Model.Commands;
using SubscriptionsService.Domain.Repositories;
using SubscriptionsService.Domain.Services;

namespace SubscriptionsService.Application.Internal.CommandServices;

public class PlanCommandService(IPlanRepository planRepository, IUnitOfWork unitOfWork) : IPlanCommandService
{
    public async Task<Plan?> Handle(CreatePlanCommand command)
    {
        if (command.Price <= 0)
        {
            throw new BadHttpRequestException("Plan price cannot be negative or less than 0");
        }
        var plan = new Plan(command.Name, command.Service, command.Price);
        await planRepository.AddAsync(plan);
        await unitOfWork.CompleteAsync();
        return plan;
    }
}