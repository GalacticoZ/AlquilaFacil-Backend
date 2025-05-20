using NotificationService.Domain.Models.Aggregates;
using NotificationService.Domain.Models.Queries;
using NotificationService.Domain.Repositories;
using NotificationService.Domain.Services;

namespace NotificationService.Application.Internal.QueryServices;

public class NotificationQueryService(INotificationRepository notificationRepository) : INotificationQueryService
{
    public async Task<IEnumerable<Notification>> Handle(GetNotificationsByUserIdQuery query)
    {
        return await notificationRepository.GetNotificationsByUserId(query.UserId);
    }
}