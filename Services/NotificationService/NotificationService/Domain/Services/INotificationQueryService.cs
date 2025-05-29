using NotificationService.Domain.Models.Aggregates;
using NotificationService.Domain.Models.Queries;

namespace NotificationService.Domain.Services;

public interface INotificationQueryService
{
    Task<IEnumerable<Notification>> Handle(GetNotificationsByUserIdQuery query);
}