using SubscriptionsService.Domain.Model.Aggregates;

namespace SubscriptionsService.Interfaces.REST.Resources;

public record InvoiceResource(int SubscriptionId, float Amount, DateTime Date);