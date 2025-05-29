namespace BookingService.Interfaces.ACL.DTOs;

public class SubscriptionDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int SubscriptionStatusId { get; set; }
    public int PlanId { get; set; }
    public string VoucherImageUrl { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
}