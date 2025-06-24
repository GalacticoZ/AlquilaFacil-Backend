using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Repositories;
using LocalsService.Infrastructure.Persistence.EFC.Configuration;
using Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LocalsService.Infrastructure.Persistence.EFC.Repositories;

public class ReportRepository(AppDbContext context) : BaseRepository<Report>(context), IReportRepository
{
    public async Task<IEnumerable<Report>> GetReportsByLocalId(int localId)
    {
        return await Context.Set<Report>().Where(report => report.LocalId == localId).ToListAsync();
    }

    public async Task<IEnumerable<Report>> GetReportsByUserId(int userId)
    {
        return await Context.Set<Report>().Where(report => report.UserId == userId).ToListAsync();
    }
}