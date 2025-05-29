using IAMService.Domain.Model.Commands;
using IAMService.Interfaces.REST.Resources;

namespace IAMService.Interfaces.REST.Transform;

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource)
    {
        return new SignUpCommand(
            resource.Username,
            resource.Password,
            resource.Email,
            resource.Name,
            resource.FatherName,
            resource.MotherName,
            resource.DateOfBirth,
            resource.DocumentNumber,
            resource.Phone);
    }
}