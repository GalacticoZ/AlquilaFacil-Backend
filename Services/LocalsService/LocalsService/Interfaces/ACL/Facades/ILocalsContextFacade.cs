using LocalsService.Domain.Model.Aggregates;

namespace LocalsService.Interfaces.ACL.Facades;

public interface ILocalsContextFacade
{

    Task<bool> LocalExists(int localId);
    
    Task<IEnumerable<Local?>> GetLocalsByUserId(int userId);
    
    Task<bool> IsLocalOwner(int userId, int localId);
    Task<int> GetLocalOwnerIdByLocalId(int localId);
}