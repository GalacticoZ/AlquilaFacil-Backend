using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Models.Aggregates;
using NotificationService.Domain.Repositories;
using NotificationService.Infrastructure.Persistence.EFC.Configuration;
using Shared.Infrastructure.Persistence.EFC.Repositories;

namespace NotificationService.Infrastructure.Persistence.EFC.Repositories;

public class NotificationRepository(AppDbContext context) : BaseRepository<Notification>(context), INotificationRepository
{
    public async Task<IEnumerable<Notification>> GetNotificationsByUserId(int userId)
    {
        return await Context.Set<Notification>().Where(n => n.UserId == userId).ToListAsync();
    }
}