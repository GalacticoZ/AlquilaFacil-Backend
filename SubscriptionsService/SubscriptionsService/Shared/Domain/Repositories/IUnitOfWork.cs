namespace SubscriptionsService.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}