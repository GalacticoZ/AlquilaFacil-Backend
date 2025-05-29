using ProfilesService.Domain.Model.Aggregates;
using ProfilesService.Domain.Model.Commands;

namespace ProfilesService.Domain.Services;

public interface IProfileCommandService
{
    public Task<Profile?> Handle(CreateProfileCommand command);
    public Task<Profile> Handle(UpdateProfileCommand command);
}