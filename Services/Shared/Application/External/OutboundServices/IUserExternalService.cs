namespace Shared.Application.External.OutboundServices;

public interface IUserExternalService
{
    Task<bool> UserExists(int userId);
}