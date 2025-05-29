using Microsoft.EntityFrameworkCore;
using ProfilesService.Domain.Model.Aggregates;
using ProfilesService.Domain.Repositories;
using ProfilesService.Shared.Infrastructure.Persistence.EFC.Configuration;
using ProfilesService.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace ProfilesService.Infrastructure.Persistence.EFC.Repositories;


public class ProfileRepository(AppDbContext context) : BaseRepository<Profile>(context), IProfileRepository
{
    public async Task<Profile?> FindByUserIdAsync(int userId)
    {
        return await context.Set<Profile>().FirstOrDefaultAsync(p => p.UserId == userId);
    }
}