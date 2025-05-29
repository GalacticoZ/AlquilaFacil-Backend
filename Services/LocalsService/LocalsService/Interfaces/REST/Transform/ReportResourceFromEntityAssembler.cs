using LocalsService.Domain.Model.Aggregates;
using LocalsService.Interfaces.REST.Resources;

namespace LocalsService.Interfaces.REST.Transform;

public class ReportResourceFromEntityAssembler
{
    public static ReportResource ToResourceFromEntity(Report? report)
    {
        return new ReportResource(
            report.Id,
            report.LocalId,
            report.Title,
            report.UserId,
            report.Description
        );
    }
}