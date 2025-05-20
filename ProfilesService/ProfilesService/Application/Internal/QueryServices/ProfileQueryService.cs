using ProfilesService.Domain.Model.Aggregates;
using ProfilesService.Domain.Model.Queries;
using ProfilesService.Domain.Repositories;
using ProfilesService.Domain.Services;

namespace ProfilesService.Application.Internal.QueryServices;

public class ProfileQueryService(IProfileRepository profileRepository) : IProfileQueryService
{

    public async Task<Profile?> Handle(GetProfileByUserIdQuery query)
    {
        return await profileRepository.FindByUserIdAsync(query.UserId);
    }
    
    public async Task<List<string>> Handle(GetProfileBankAccountsByUserIdQuery query)
    {
        var bankAccounts = new List<string>();
        var profile = await profileRepository.FindByUserIdAsync(query.UserId);
        if (profile.Id != 0) 
        {
            bankAccounts.Add(profile.BankAccountNumber);
            bankAccounts.Add(profile.InterbankAccountNumber);
        }
        return bankAccounts;
    }
}