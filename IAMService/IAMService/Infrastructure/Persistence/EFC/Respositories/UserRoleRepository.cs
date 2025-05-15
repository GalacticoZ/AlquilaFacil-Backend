using IAMService.Domain.Model.Entities;
using IAMService.Domain.Model.ValueObjects;
using IAMService.Domain.Repositories;
using IAMService.Shared.Infrastructure.Persistence.EFC.Configuration;
using IAMService.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IAMService.Infrastructure.Persistence.EFC.Respositories;

public class UserRoleRepository(AppDbContext context) : BaseRepository<UserRole>(context), IUserRoleRepository
{
    public async Task<bool> ExistsUserRole(EUserRoles role)
    {
        return await Context.Set<UserRole>().AnyAsync(userRole => userRole.Role == role.ToString());
    }
}
