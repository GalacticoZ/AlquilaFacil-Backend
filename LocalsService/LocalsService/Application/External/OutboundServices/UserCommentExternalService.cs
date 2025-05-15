using LocalsService.Interfaces.ACL.Facades;

namespace LocalsService.Application.External.OutboundServices;

public class UserCommentExternalService (IIamContextFacade iamContextFacade) : IUserCommentExternalService
{
    public Task<bool> UserExists(int userId)
    {
        return iamContextFacade.UserExists(userId);
    }    
}