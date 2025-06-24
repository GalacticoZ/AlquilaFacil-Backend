using IAMService.Domain.Model.Aggregates;
using Shared.Domain.Repositories;

namespace IAMService.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByEmailAsync (string email);
    Task<bool> ExistsByUsername(string username);
    Task<string?> GetUsernameByIdAsync(int userId);
    bool ExistsById(int userId);
}