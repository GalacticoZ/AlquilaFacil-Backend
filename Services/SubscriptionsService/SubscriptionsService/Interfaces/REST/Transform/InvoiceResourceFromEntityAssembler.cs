using SubscriptionsService.Domain.Model.Entities;
using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Interfaces.REST.Resources;

namespace SubscriptionsService.Interfaces.REST.Transform;

public static class InvoiceResourceFromEntityAssembler
{
    public static InvoiceResource ToResourceFromEntity(Invoice entity)
    {
        return new InvoiceResource(entity.SubscriptionId,
            entity.Amount,
            entity.Date);
    }
}