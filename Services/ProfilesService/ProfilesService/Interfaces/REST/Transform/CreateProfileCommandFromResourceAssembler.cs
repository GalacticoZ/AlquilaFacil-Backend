using ProfilesService.Domain.Model.Commands;
using ProfilesService.Interfaces.REST.Resources;

namespace ProfilesService.Interfaces.REST.Transform;

public class CreateProfileCommandFromResourceAssembler
{
    public static CreateProfileCommand ToCommandFromResource(CreateProfileResource resource)
    {
        return new CreateProfileCommand(
            resource.Name, 
            resource.FatherName, 
            resource.MotherName, 
            resource.DateOfBirth,
            resource.DocumentNumber, 
            resource.Phone,
            resource.UserId
        );
    }
}