using IAMService.Domain.Model.Commands;

namespace IAMService.Domain.Services;

public interface ISeedUserRoleCommandService
{
    Task Handle(SeedUserRolesCommand command);
}