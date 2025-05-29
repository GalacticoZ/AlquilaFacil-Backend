namespace LocalsService.Application.External.OutboundServices;

public interface IUserCommentExternalService
{
    Task<bool> UserExists(int userId);
}