using Shared.Interfaces.ACL.DTOs;

namespace Shared.Interfaces.ACL.Facades;

public interface ILocalsContextFacade
{
    Task<bool> LocalExists(int localId);
    Task<IEnumerable<LocalDTO>> GetLocalsByUserId(int userId);
    Task<bool> IsLocalOwner(int userId, int localId);
    Task<int> GetLocalOwnerIdByLocalId(int localId);
}