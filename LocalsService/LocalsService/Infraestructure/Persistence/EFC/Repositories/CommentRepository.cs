using System.Reflection.Metadata;
using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Repositories;
using LocalsService.Shared.Infrastructure.Persistence.EFC.Configuration;
using LocalsService.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LocalsService.Infraestructure.Persistence.EFC.Repositories;

public class CommentRepository(AppDbContext context)
    : BaseRepository<Comment>(context), ICommentRepository
{
    public async Task<IEnumerable<Comment>> GetAllCommentsByLocalId(int localId)
    {
        return await Context.Set<Comment>().Where(c => c.LocalId == localId).ToListAsync();
    }
}