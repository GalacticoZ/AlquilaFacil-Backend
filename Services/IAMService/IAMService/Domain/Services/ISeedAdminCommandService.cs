using IAMService.Domain.Model.Commands;

namespace IAMService.Domain.Services;

public interface ISeedAdminCommandService
{
    Task Handle(SeedAdminCommand command);
}