namespace SubscriptionsService.Interfaces.REST.Resources;

public record CreateInvoiceResource(int SubscriptionId, float Amount, DateTime Date);