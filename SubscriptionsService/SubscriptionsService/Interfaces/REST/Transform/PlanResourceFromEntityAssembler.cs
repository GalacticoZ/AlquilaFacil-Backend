using SubscriptionsService.Domain.Model.Entities;
using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Interfaces.REST.Resources;

namespace SubscriptionsService.Interfaces.REST.Transform;

public static class PlanResourceFromEntityAssembler
{
    public static PlanResource ToResourceFromEntity(Plan entity)
    {
        return new PlanResource(entity.Id, entity.Name, entity.Service, entity.Price);
    }

}