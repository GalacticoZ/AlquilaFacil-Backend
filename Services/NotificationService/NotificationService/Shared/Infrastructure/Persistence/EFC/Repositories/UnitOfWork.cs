using NotificationService.Shared.Domain.Repositories;
using NotificationService.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace NotificationService.Shared.Infrastructure.Persistence.EFC.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context) => _context = context;
    public async Task CompleteAsync() => await _context.SaveChangesAsync();
}