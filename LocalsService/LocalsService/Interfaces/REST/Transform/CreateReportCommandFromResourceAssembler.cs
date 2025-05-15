using LocalsService.Domain.Model.Commands;
using LocalsService.Interfaces.REST.Resources;

namespace LocalsService.Interfaces.REST.Transform;

public class CreateReportCommandFromResourceAssembler
{
    public static CreateReportCommand ToCommandFromResource(CreateReportResource resource)
    {
        return new CreateReportCommand(
             resource.LocalId,
             resource.Title,
             resource.UserId,
             resource.Description
        );
    }
}