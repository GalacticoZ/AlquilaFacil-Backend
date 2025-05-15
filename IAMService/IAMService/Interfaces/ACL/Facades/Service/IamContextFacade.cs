using IAMService.Domain.Model.Commands;
using IAMService.Domain.Model.Queries;
using IAMService.Domain.Services;
using IAMService.Interfaces.ACL.Facades;

namespace IAMService.Interfaces.ACL.Facades.Service;

public class IamContextFacade(IUserCommandService userCommandService, IUserQueryService userQueryService) : IIamContextFacade
{
    public async Task<int> FetchUserIdByUsername(string username)
    {
        var getUserByUsernameQuery = new GetUserByEmailQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery);
        return result?.Id ?? 0;
    }

    public async Task<string> FetchUsernameByUserId(int userId)
    {
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var result = await userQueryService.Handle(getUserByIdQuery);
        return result?.Username ?? string.Empty;
    }

    public bool UsersExists(int userId)
    {
        return userQueryService.Handle(new UserExistsQuery(userId));
    }
}