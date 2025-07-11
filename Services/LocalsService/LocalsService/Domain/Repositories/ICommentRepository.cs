using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Model.Entities;
using LocalsService.Domain.Model.ValueObjects;
using Shared.Domain.Repositories;

namespace LocalsService.Domain.Repositories;

public interface ICommentRepository : IBaseRepository<Comment>
{ 
    Task<IEnumerable<Comment>> GetAllCommentsByLocalId(int localId);
}