using LocalsService.Domain.Model.Aggregates;
using LocalsService.Interfaces.REST.Resources;

namespace LocalsService.Interfaces.REST.Transform;

public static class CommentResourceFromEntityAssembler
{
    public static CommentResource ToResourceFromEntity(Comment entity)
    {
        return new CommentResource(entity.Id, entity.UserId, entity.LocalId, entity.CommentText, entity.CommentRating);
    }
}