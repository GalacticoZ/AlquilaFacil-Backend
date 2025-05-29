using BookingService.Interfaces.ACL.DTOs;

namespace BookingService.Interfaces.ACL.Facades;

public interface ILocalsContextFacade
{
    Task<bool> LocalExists(int localId);
    Task<IEnumerable<LocalDTO>> GetLocalsByUserId(int userId);
    Task<bool> IsLocalOwner(int userId, int localId);
    Task<int> GetLocalOwnerIdByLocalId(int localId);
}