using IAMService.Domain.Model.Commands;
using IAMService.Domain.Model.Entities;
using IAMService.Domain.Model.ValueObjects;
using IAMService.Domain.Repositories;
using IAMService.Domain.Services;
using IAMService.Shared.Domain.Repositories;

namespace IAMService.Application.Internal.CommandServices;

public class SeedUserRoleCommandService(IUserRoleRepository repository, IUnitOfWork unitOfWork) : ISeedUserRoleCommandService
{
    public async Task Handle(SeedUserRolesCommand command)
    {
        foreach (EUserRoles role in Enum.GetValues(typeof(EUserRoles)))
        {
            if (await repository.ExistsUserRole(role)) continue;
            var userRole = new UserRole(role.ToString());
            await repository.AddAsync(userRole);
        }

        await unitOfWork.CompleteAsync();
    }
}