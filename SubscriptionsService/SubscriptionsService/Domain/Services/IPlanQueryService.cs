using SubscriptionsService.Domain.Model.Entities;
using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Domain.Model.Queries;

namespace SubscriptionsService.Domain.Services;

public interface IPlanQueryService
{
    Task<IEnumerable<Plan>> Handle(GetAllPlansQuery query);
}