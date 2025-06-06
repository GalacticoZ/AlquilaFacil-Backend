using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Model.Commands;
using LocalsService.Domain.Repositories;
using LocalsService.Domain.Services;
using Shared.Application.External.OutboundServices;
using Shared.Domain.Repositories;

namespace LocalsService.Application.Internal.CommandServices;

public class ReportCommandService (IReportRepository reportRepository, IUserExternalService userExternalService, IUnitOfWork unitOfWork) : IReportCommandService
{
    public async Task<Report?> Handle(CreateReportCommand command)
    {
        var userExists = await userExternalService.UserExists(command.UserId);
        
        if (!userExists)
        {
            throw new KeyNotFoundException("There are no users matching the id specified");
        }
        
        var report = new Report(command);
        await reportRepository.AddAsync(report);
        await unitOfWork.CompleteAsync();
        return report;
    }

    public async Task<Report?> Handle(DeleteReportCommand command)
    {
        var reportToDelete =  await reportRepository.FindByIdAsync(command.Id);
        if (reportToDelete == null)
        {
            throw new KeyNotFoundException("Report not found");
        }
        reportRepository.Remove(reportToDelete);
        await unitOfWork.CompleteAsync();
        return reportToDelete;
    }
}