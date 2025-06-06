using Shared.Interfaces.ACL.DTOs;
using Shared.Interfaces.ACL.Facades;

namespace BookingService.Application.External.OutboundServices;


public class ReservationLocalExternalService(ILocalsContextFacade localsContextFacade) : IReservationLocalExternalService
{
    public Task<bool> LocalReservationExists(int reservationId)
    {
        return localsContextFacade.LocalExists(reservationId);
    }

    public async Task<IEnumerable<LocalDTO>> GetLocalsByUserId(int userId)
    {
        return await localsContextFacade.GetLocalsByUserId(userId);
    }

    public async Task<bool> IsLocalOwner(int userId, int localId)
    {
        return await localsContextFacade.IsLocalOwner(userId, localId);
    }
    
    public async Task<int> GetOwnerIdByLocalId(int localId)
    {
        return await localsContextFacade.GetLocalOwnerIdByLocalId(localId);
    }
}