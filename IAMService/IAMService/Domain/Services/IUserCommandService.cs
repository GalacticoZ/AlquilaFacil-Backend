using IAMService.Domain.Model.Aggregates;
using IAMService.Domain.Model.Commands;

namespace IAMService.Domain.Services;

public interface IUserCommandService
{
    Task<(User user, string token)> Handle(SignInCommand command);
    Task<User?> Handle(SignUpCommand command);
    Task<User?> Handle(UpdateUsernameCommand command);
    
}