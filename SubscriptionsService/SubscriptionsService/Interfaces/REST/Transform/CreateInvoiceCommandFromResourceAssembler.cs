using SubscriptionsService.Domain.Model.Commands;
using SubscriptionsService.Interfaces.REST.Resources;

namespace SubscriptionsService.Interfaces.REST.Transform;

public static class CreateInvoiceCommandFromResourceAssembler
{
    public static CreateInvoiceCommand ToCommandFromResource(CreateInvoiceResource resource)
    {
        return new CreateInvoiceCommand(resource.SubscriptionId, resource.Amount, resource.Date);
    }
}