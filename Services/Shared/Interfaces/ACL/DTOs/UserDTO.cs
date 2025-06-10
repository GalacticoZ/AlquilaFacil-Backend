namespace Shared.Interfaces.ACL.DTOs;

public class UserDTO
{
    public int Id { get; set; }
    public string Username { get; private set; }
    public string Email { get; set; }
}