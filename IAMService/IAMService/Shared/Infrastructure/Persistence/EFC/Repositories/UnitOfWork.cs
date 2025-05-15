using IAMService.Shared.Domain.Repositories;
using IAMService.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace IAMService.Shared.Infrastructure.Persistence.EFC.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context) => _context = context;
    public async Task CompleteAsync() => await _context.SaveChangesAsync();
}