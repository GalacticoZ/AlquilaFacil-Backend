namespace SubscriptionsService.Interfaces.REST.Resources;

public record SubscriptionResource(int Id, int UserId, int PlanId, int SubscriptionStatusId, string VoucherImageUrl);