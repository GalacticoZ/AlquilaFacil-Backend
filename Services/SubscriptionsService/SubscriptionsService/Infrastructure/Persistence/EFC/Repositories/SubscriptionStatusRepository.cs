using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Persistence.EFC.Repositories;
using SubscriptionsService.Domain.Model.Entities;
using SubscriptionsService.Domain.Model.ValueObjects;
using SubscriptionsService.Domain.Repositories;
using SubscriptionsService.Infrastructure.Persistence.EFC.Configuration;

namespace SubscriptionsService.Infrastructure.Persistence.EFC.Repositories;

public class SubscriptionStatusRepository(AppDbContext context) : BaseRepository<SubscriptionStatus>(context), ISubscriptionStatusRepository
{
    public async Task<bool> ExistsBySubscriptionStatus(ESubscriptionStatus subscriptionStatus)
    {
        return await Context.Set<SubscriptionStatus>().AnyAsync(x => x.Status == subscriptionStatus.ToString());
    }
}