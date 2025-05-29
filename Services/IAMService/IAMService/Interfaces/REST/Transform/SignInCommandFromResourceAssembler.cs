using IAMService.Domain.Model.Commands;
using IAMService.Interfaces.REST.Resources;

namespace IAMService.Interfaces.REST.Transform;

public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource)
    {
        return new SignInCommand(resource.Email, resource.Password);
    }
}