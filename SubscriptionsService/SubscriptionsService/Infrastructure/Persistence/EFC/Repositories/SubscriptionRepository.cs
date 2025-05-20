using Microsoft.EntityFrameworkCore;
using SubscriptionsService.Shared.Infrastructure.Persistence.EFC.Configuration;
using SubscriptionsService.Shared.Infrastructure.Persistence.EFC.Repositories;
using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Domain.Repositories;

namespace SubscriptionsService.Infrastructure.Persistence.EFC.Repositories;

public class SubscriptionRepository(AppDbContext context)
    : BaseRepository<Subscription>(context), ISubscriptionRepository
{
    public async Task<Subscription?> FindByUserIdAsync(int userId)
    {
        return await context.Set<Subscription>()
            .LastOrDefaultAsync(s => s.UserId == userId);
    }
    
    public async Task<IEnumerable<Subscription>> FindByUserIdsListAsync(List<int> userIdsList)
    {
        return await context.Set<Subscription>()
            .Where(s => userIdsList.Contains(s.UserId))
            .ToListAsync();
    }
}