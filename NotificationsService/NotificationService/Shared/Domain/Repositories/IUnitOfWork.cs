namespace NotificationService.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}