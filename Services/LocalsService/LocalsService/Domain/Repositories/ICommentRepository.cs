using LocalsService.Domain.Model.Entities;
using LocalsService.Domain.Model.ValueObjects;
using LocalsService.Domain.Model.Aggregates;
using Shared.Domain.Repositories;

namespace LocalsService.Domain.Repositories;

public interface ICommentRepository : IBaseRepository<Comment>
{
        Task<IEnumerable<Comment>> GetAllCommentsByLocalId(int localId);
}