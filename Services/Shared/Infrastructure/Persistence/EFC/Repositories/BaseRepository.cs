using Shared.Domain.Repositories;
using Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Shared.Infrastructure.Persistence.EFC.Repositories;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class
{

    protected readonly BaseDbContext Context;
    
    protected BaseRepository (BaseDbContext context) => Context = context;

    public async Task AddAsync(TEntity entity) => await Context.Set<TEntity>().AddAsync(entity);

    public async Task<TEntity?> FindByIdAsync(int id) => await Context.Set<TEntity>().FindAsync(id);

    public void Update(TEntity entity) => Context.Set<TEntity>().Update(entity);

    public void Remove(TEntity entity) => Context.Set<TEntity>().Remove(entity);

    public async Task<IEnumerable<TEntity>> ListAsync() => await Context.Set<TEntity>().ToListAsync();
}