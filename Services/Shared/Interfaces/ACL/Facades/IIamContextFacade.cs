namespace Shared.Interfaces.ACL.Facades;

public interface IIamContextFacade
{
    Task<bool> UserExists(int userId);
}