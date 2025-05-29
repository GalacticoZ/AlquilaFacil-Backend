using LocalsService.Domain.Model.ValueObjects;
using LocalsService.Domain.Model.Aggregates;
using LocalsService.Shared.Domain.Repositories;

namespace LocalsService.Domain.Repositories;

public interface ILocalRepository : IBaseRepository<Local>
{
   HashSet<string> GetAllDistrictsAsync();
   
   Task<IEnumerable<Local>> GetLocalsByCategoryIdAndCapacityrange(int categoryId, int minCapacity, int maxCapacity);
   
   Task<IEnumerable<Local>> GetLocalsByUserIdAsync(int userId);
   Task<bool> IsOwnerAsync(int userId, int localId);
   Task<int?> GetLocalOwnerIdByLocalId(int localId);
}