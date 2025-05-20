using SubscriptionsService.Shared.Domain.Repositories;
using SubscriptionsService.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace SubscriptionsService.Shared.Infrastructure.Persistence.EFC.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context) => _context = context;
    public async Task CompleteAsync() => await _context.SaveChangesAsync();
}