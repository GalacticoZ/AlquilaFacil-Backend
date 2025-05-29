using LocalsService.Domain.Model.Aggregates;
using LocalsService.Shared.Domain.Repositories;

namespace LocalsService.Domain.Repositories;

public interface IReportRepository : IBaseRepository<Report>
{
    Task<IEnumerable<Report>> GetReportsByLocalId(int localId);
    Task<IEnumerable<Report>> GetReportsByUserId(int userId);
}