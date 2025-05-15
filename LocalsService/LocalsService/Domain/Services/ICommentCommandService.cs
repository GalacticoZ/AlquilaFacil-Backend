using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Model.Commands;

namespace LocalsService.Locals.Domain.Services;

public interface ICommentCommandService
{
    Task<Comment?> Handle(CreateCommentCommand command);
}