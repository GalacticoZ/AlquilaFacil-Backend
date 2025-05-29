using ProfilesService.Domain.Model.Commands;
using ProfilesService.Interfaces.REST.Resources;

namespace ProfilesService.Interfaces.REST.Transform;

public class UpdateProfileCommandFromResourceAssembler
{
    public static UpdateProfileCommand ToCommandFromResource(int userId, UpdateProfileResource resource)
    {
        return new UpdateProfileCommand(
            resource.Name, 
            resource.FatherName, 
            resource.MotherName, 
            resource.DateOfBirth,
            resource.DocumentNumber, 
            resource.Phone,
            resource.BankAccountNumber,
            resource.InterbankAccountNumber,
            userId
            );
    }
}