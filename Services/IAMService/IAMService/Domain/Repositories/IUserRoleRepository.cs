using IAMService.Domain.Model.Entities;
using IAMService.Domain.Model.ValueObjects;
using Shared.Domain.Repositories;

namespace IAMService.Domain.Repositories;

public interface IUserRoleRepository : IBaseRepository<UserRole>
{
    Task<bool> ExistsUserRole(EUserRoles role);
}