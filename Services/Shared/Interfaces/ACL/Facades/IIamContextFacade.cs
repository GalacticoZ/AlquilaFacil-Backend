using Shared.Interfaces.ACL.DTOs;

namespace Shared.Interfaces.ACL.Facades;

public interface IIamContextFacade
{
    Task<bool> UserExists(int userId);

    Task<UserDTO> FetchUser(int userId);
}