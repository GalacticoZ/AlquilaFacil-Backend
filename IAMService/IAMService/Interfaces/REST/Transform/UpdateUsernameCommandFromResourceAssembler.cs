using IAMService.Domain.Model.Commands;
using IAMService.Interfaces.REST.Resources;

namespace IAMService.Interfaces.REST.Transform;

public static class UpdateUsernameCommandFromResourceAssembler
{
    public static UpdateUsernameCommand ToUpdateUsernameCommand(int id, UpdateUsernameResource resource)
    {
        return new UpdateUsernameCommand(id, resource.Username);
    }
}