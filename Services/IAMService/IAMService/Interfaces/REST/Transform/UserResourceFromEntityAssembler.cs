using System.Diagnostics;
using IAMService.Domain.Model.Aggregates;
using IAMService.Interfaces.REST.Resources;

namespace IAMService.Interfaces.REST.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User? user)
    {
        Console.WriteLine("User Name is " + user?.Username);
        Debug.Assert(user != null, nameof(user) + " != null");
        return new UserResource(user.Id, user.Username);
    }
}