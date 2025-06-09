using System.Net.Mime;
using LocalsService.Domain.Model.Commands;
using LocalsService.Domain.Model.Queries;
using LocalsService.Domain.Services;
using LocalsService.Interfaces.REST.Resources;
using LocalsService.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.REST.Resources;

namespace LocalsService.Interfaces.REST;

/// <summary>
/// Controller for locals report management
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ReportController(IReportQueryService reportQueryService, IReportCommandService reportCommandService) : ControllerBase
{
    /// <summary>
    /// POST endpoint to create a new report
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReportResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateReport([FromBody] CreateReportResource createReportResource)
    {
        try
        {
            var command = CreateReportCommandFromResourceAssembler.ToCommandFromResource(createReportResource);
            var report = await reportCommandService.Handle(command);
            var reportResource = ReportResourceFromEntityAssembler.ToResourceFromEntity(report);
            return StatusCode(201, reportResource);
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(new { Error = ex.Message }); // 400
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Internal server error", Details = ex.Message }); // 500
        }
    }

    /// <summary>
    /// GET endpoint to retrieve all reports for a specific user
    /// </summary>
    [HttpGet("get-by-user-id/{userId:int}")]
    [ProducesResponseType(typeof(ReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReportsByUserId(int userId)
    {
        try
        {
            var query = new GetReportsByUserIdQuery(userId);
            var reports = await reportQueryService.Handle(query);
            var reportResources = reports.Select(ReportResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(reportResources);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// GET endpoint to retrieve all reports for a specific local
    /// </summary>
    [HttpGet("get-by-local-id/{localId:int}")]
    [ProducesResponseType(typeof(ReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReportsByLocalId(int localId)
    {
        try
        {
            var query = new GetReportsByLocalIdQuery(localId);
            var reports = await reportQueryService.Handle(query);
            var reportResources = reports.Select(ReportResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(reportResources);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// DELETE endpoint to remove a specific report
    /// </summary>
    [HttpDelete("{reportId:int}")]
    [ProducesResponseType(typeof(ReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReport(int reportId)
    {
        try
        {
            var command = new DeleteReportCommand(reportId);
            var reportDeleted = await reportCommandService.Handle(command);
            return StatusCode(200, reportDeleted);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
}