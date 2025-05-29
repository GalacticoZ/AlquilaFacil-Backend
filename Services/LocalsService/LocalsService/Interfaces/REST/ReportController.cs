using System.Net.Mime;
using LocalsService.Domain.Model.Commands;
using LocalsService.Domain.Model.Queries;
using LocalsService.Domain.Services;
using LocalsService.Interfaces.REST.Resources;
using LocalsService.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LocalsService.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ReportController(IReportQueryService reportQueryService, IReportCommandService reportCommandService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateReport([FromBody] CreateReportResource createReportResource)
    {
        var command = CreateReportCommandFromResourceAssembler.ToCommandFromResource(createReportResource);
        var report = await reportCommandService.Handle(command);
        var reportResource = ReportResourceFromEntityAssembler.ToResourceFromEntity(report);
        return StatusCode(201, reportResource);
    }
    
    [HttpGet("get-by-user-id/{userId:int}")]
    public async Task<IActionResult> GetReportsByUserId(int userId)
    {
        var query = new GetReportsByUserIdQuery(userId);
        var reports = await reportQueryService.Handle(query);
        var reportResources = reports.Select(ReportResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(reportResources);
    }
    
    [HttpGet("get-by-local-id/{localId:int}")]
    public async Task<IActionResult> GetReportsByLocalId(int localId)
    {
        var query = new GetReportsByLocalIdQuery(localId);
        var reports = await reportQueryService.Handle(query);
        var reportResources = reports.Select(ReportResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(reportResources);
    }
    
    [HttpDelete("{reportId:int}")]
    public async Task<IActionResult> DeleteReport(int reportId)
    {
        var command = new DeleteReportCommand(reportId);
        var reportDeleted = await reportCommandService.Handle(command);
        return StatusCode(200, reportDeleted);
    }
}