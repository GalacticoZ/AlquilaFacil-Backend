using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Model.Queries;
using LocalsService.Domain.Repositories;
using LocalsService.Domain.Services;

namespace LocalsService.Application.Internal.QueryServices;

public class CommentQueryService (ICommentRepository commentRepository) : ICommentQueryService
{
    public Task<IEnumerable<Comment>> Handle(GetAllCommentsByLocalIdQuery query)
    {
        return commentRepository.GetAllCommentsByLocalId(query.LocalId);
    }
}