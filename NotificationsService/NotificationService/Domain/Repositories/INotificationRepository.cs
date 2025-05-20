using NotificationService.Domain.Models.Aggregates;
using NotificationService.Shared.Domain.Repositories;

namespace NotificationService.Domain.Repositories;

public interface INotificationRepository : IBaseRepository<Notification>
{
    Task<IEnumerable<Notification>> GetNotificationsByUserId(int userId);
}