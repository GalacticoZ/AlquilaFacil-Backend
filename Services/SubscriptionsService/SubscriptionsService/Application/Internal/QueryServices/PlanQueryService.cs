using SubscriptionsService.Domain.Model.Entities;
using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Domain.Model.Queries;
using SubscriptionsService.Domain.Repositories;
using SubscriptionsService.Domain.Services;

namespace SubscriptionsService.Application.Internal.QueryServices;

public class PlanQueryService(IPlanRepository planRepository) : IPlanQueryService
{
    public async Task<IEnumerable<Plan>> Handle(GetAllPlansQuery query)
    {
        return await planRepository.ListAsync();
    }
}