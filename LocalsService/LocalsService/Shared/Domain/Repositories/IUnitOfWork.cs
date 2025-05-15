namespace LocalsService.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}