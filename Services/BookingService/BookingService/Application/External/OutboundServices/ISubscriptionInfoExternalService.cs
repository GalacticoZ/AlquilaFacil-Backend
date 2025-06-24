using Shared.Interfaces.ACL.DTOs;

namespace BookingService.Application.External.OutboundServices;

public interface ISubscriptionInfoExternalService
{
    Task<IEnumerable<SubscriptionDTO>> GetSubscriptionByUserIdsList(List<int> usersId);
}