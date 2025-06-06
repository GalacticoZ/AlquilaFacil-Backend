using ProfilesService.Domain.Model.Aggregates;
using ProfilesService.Domain.Model.Commands;
using ProfilesService.Domain.Repositories;
using ProfilesService.Domain.Services;
using Shared.Domain.Repositories;

namespace ProfilesService.Application.Internal.CommandServices;

public class ProfileCommandService(IProfileRepository profileRepository, IUnitOfWork unitOfWork) : IProfileCommandService
{
    public async Task<Profile?> Handle(CreateProfileCommand command)
    {
        var profile = new Profile(command);
        await profileRepository.AddAsync(profile);
        await unitOfWork.CompleteAsync();
        return profile;
    }

    public async Task<Profile> Handle(UpdateProfileCommand command)
    {
        var profile = await profileRepository.FindByUserIdAsync(command.UserId);
        if (profile == null)
        {
            throw new KeyNotFoundException("Profile with User ID does not exist");
        }
        profile.Update(command);
        await unitOfWork.CompleteAsync();
        return profile;
    }
}