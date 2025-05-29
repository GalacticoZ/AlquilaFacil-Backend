using IAMService.Domain.Model.Aggregates;
using IAMService.Interfaces.REST.Resources;

namespace IAMService.Interfaces.REST.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(User user, string token)
    {
        return new AuthenticatedUserResource(user.Id, user.Username, token);
    }
}