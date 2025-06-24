using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Model.Queries;
using LocalsService.Domain.Repositories;
using LocalsService.Domain.Services;

namespace LocalsService.Application.Internal.QueryServices;

public class ReportQueryService (IReportRepository reportRepository) : IReportQueryService
{
    public async Task<IEnumerable<Report?>> Handle(GetReportsByLocalIdQuery query)
    {
        return await reportRepository.GetReportsByLocalId(query.LocalId);
    }

    public async Task<IEnumerable<Report?>> Handle(GetReportsByUserIdQuery query)
    {
        return await reportRepository.GetReportsByUserId(query.UserId);
    }
}