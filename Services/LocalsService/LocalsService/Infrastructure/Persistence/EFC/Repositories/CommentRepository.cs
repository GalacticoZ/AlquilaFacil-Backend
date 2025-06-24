using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Repositories;
using LocalsService.Infrastructure.Persistence.EFC.Configuration;
using Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LocalsService.Infrastructure.Persistence.EFC.Repositories;

public class CommentRepository(AppDbContext context)
    : BaseRepository<Comment>(context), ICommentRepository
{
    public async Task<IEnumerable<Comment>> GetAllCommentsByLocalId(int localId)
    {
        return await Context.Set<Comment>().Where(c => c.LocalId == localId).ToListAsync();
    }
}