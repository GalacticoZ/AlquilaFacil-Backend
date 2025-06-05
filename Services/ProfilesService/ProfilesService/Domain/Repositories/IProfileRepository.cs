using ProfilesService.Domain.Model.Aggregates;
using Shared.Domain.Repositories;

namespace ProfilesService.Domain.Repositories;

public interface IProfileRepository : IBaseRepository<Profile>
{
    Task<Profile?> FindByUserIdAsync(int userId);
}