namespace SubscriptionsService.Domain.Model.Queries;

public record GetSubscriptionsByUserIdQuery(List<int> UserIds);