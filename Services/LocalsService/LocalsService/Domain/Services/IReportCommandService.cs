
using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Model.Commands;

namespace LocalsService.Domain.Services;

public interface IReportCommandService
{
    Task<Report?> Handle(CreateReportCommand command);
    Task<Report?> Handle(DeleteReportCommand command);
}