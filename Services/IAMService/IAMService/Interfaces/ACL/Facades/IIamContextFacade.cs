namespace IAMService.Interfaces.ACL.Facades;

public interface IIamContextFacade
{
    Task<int> FetchUserIdByUsername(string username);
    Task<string> FetchUsernameByUserId(int userId);
    bool UsersExists(int userId);
}