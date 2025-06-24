using System.Reflection.Metadata;
using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Model.Queries;

namespace LocalsService.Domain.Services;

public interface IReportQueryService
{
    Task<IEnumerable<Report?>> Handle(GetReportsByLocalIdQuery query);
    Task<IEnumerable<Report?>> Handle(GetReportsByUserIdQuery query);
}