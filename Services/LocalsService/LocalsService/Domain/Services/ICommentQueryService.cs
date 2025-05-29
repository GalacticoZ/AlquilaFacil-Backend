using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Model.Queries;

namespace LocalsService.Domain.Services;

public interface ICommentQueryService
{
    Task<IEnumerable<Comment>> Handle(GetAllCommentsByLocalIdQuery query);
}