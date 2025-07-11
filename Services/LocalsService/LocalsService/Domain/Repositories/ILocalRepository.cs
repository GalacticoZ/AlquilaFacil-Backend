using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Model.ValueObjects;
using Shared.Domain.Repositories;

namespace LocalsService.Domain.Repositories;

public interface ILocalRepository : IBaseRepository<Local>
{
   Task<IEnumerable<Local>> GetLocalsAsync();
   Task<Local?> GetLocalByIdAsync(int localId);
   Task<HashSet<string>> GetAllDistrictsAsync();
   Task<IEnumerable<Local>> GetLocalsByCategoryIdAndCapacityRange(int categoryId, int minCapacity, int maxCapacity);
   Task<IEnumerable<Local>> GetLocalsByUserIdAsync(int userId);
   Task<bool> IsOwnerAsync(int userId, int localId);
   Task<int?> GetLocalOwnerIdByLocalId(int localId);
}