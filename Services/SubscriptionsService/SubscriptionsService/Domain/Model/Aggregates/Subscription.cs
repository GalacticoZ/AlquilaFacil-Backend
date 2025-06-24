using SubscriptionsService.Domain.Model.Entities;
using SubscriptionsService.Domain.Model.Commands;
using SubscriptionsService.Domain.Model.ValueObjects;

namespace SubscriptionsService.Domain.Model.Aggregates;

public partial class Subscription
{
    public int Id { get; set; }
    public int UserId { get; set; }
    
    public int SubscriptionStatusId { get; set; }
    public int PlanId { get; set; }
    public string VoucherImageUrl { get; set; }

    public Subscription()
    {
        UserId = 0;
        PlanId = 0;
        VoucherImageUrl = "";
    }

    public Subscription(CreateSubscriptionCommand command)
    {
        UserId = command.UserId;
        PlanId = command.PlanId;
        VoucherImageUrl = command.VoucherImageUrl;
        SubscriptionStatusId = (int)ESubscriptionStatus.Pending;
    }
    
    public void ActiveSubscriptionStatus()
    {
        SubscriptionStatusId = (int)ESubscriptionStatus.Active;
    }
}