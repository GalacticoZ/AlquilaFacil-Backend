using ProfilesService.Domain.Model.Aggregates;
using ProfilesService.Domain.Model.Queries;

namespace ProfilesService.Domain.Services;

public interface IProfileQueryService
{
    Task<Profile?> Handle(GetProfileByUserIdQuery query);

    Task<List<string>> Handle(GetProfileBankAccountsByUserIdQuery query);
}