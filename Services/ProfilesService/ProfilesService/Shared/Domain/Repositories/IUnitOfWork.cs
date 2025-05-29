namespace ProfilesService.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}