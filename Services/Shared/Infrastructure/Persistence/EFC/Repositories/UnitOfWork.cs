using Shared.Domain.Repositories;
using Shared.Infrastructure.Persistence.EFC.Configuration;

namespace Shared.Infrastructure.Persistence.EFC.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly BaseDbContext _context;

    public UnitOfWork(BaseDbContext context) => _context = context;
    public async Task CompleteAsync() => await _context.SaveChangesAsync();
}