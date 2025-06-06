using Shared.Interfaces.ACL.Facades;

namespace Shared.Application.External.OutboundServices;

public class UserExternalService(IIamContextFacade iamContextFacade) : IUserExternalService
{
    public Task<bool> UserExists(int userId)
    {
        return iamContextFacade.UserExists(userId);
    }
}
