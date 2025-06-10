using Shared.Interfaces.ACL.DTOs;

namespace Shared.Application.External.OutboundServices;

public interface IUserExternalService
{
    Task<bool> UserExists(int userId);
    
    Task<UserDTO> FetchUser(int userId);
}