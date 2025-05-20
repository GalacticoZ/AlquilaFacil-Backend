using ProfilesService.Domain.Model.Commands;
using ProfilesService.Domain.Services;

namespace ProfilesService.Interfaces.ACL.Services;

public class ProfilesContextFacade(IProfileCommandService profileCommandService) : IProfilesContextFacade
{
    public async Task<int> CreateProfile(
        string name, 
        string fatherName, 
        string motherName, 
        string dateOfBirth, 
        string documentNumber,
        string phone, 
        int userId
        )
    {
        var createProfileCommand = new CreateProfileCommand(name, fatherName, motherName, dateOfBirth, documentNumber, phone, userId);
        var profile = await profileCommandService.Handle(createProfileCommand);
        return profile?.Id ?? 0;
    }
    
    public async Task<int> UpdateProfile(
        
        string name, 
        string fatherName, 
        string motherName, 
        string dateOfBirth, 
        string documentNumber,
        string phone,
        string bankAccountNumber,
        string interbankAccountNumber,
        int userId 
    )
    {
        var updateProfileCommand = new UpdateProfileCommand(name, fatherName, motherName, dateOfBirth, documentNumber, phone, bankAccountNumber, interbankAccountNumber, userId);
        var profile = await profileCommandService.Handle(updateProfileCommand);
        return profile?.Id ?? 0;
    }
}