using Shared.Interfaces.ACL.DTOs;

namespace BookingService.Application.External.OutboundServices;

public interface IReservationLocalExternalService
{
    Task<bool> LocalReservationExists(int reservationId);
    Task<IEnumerable<LocalDTO?>> GetLocalsByUserId(int userId);
    Task<bool> IsLocalOwner(int userId, int localId);
    Task<int> GetOwnerIdByLocalId(int localId);
}