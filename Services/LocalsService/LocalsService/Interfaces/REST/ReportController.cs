using System.Net.Mime;
using LocalsService.Domain.Model.Commands;
using LocalsService.Domain.Model.Queries;
using LocalsService.Domain.Services;
using LocalsService.Interfaces.REST.Resources;
using LocalsService.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shared.Interfaces.REST.Resources;

namespace LocalsService.Interfaces.REST;

/// <summary>
/// Controller for locals report management
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
public class ReportController(IReportQueryService reportQueryService, IReportCommandService reportCommandService) : ControllerBase
{
    /// <summary>
    /// POST endpoint to create a new report
    /// </summary>
    /// <param name="createReportResource">Data of the report to create</param>
    /// <returns>Created report</returns>
    /// <response code="201">Report successfully created</response>
    /// <response code="400">Invalid input data</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">Related resource not found</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReportResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateReport([FromBody] CreateReportResource createReportResource)
    {
        try
        {
            var command = CreateReportCommandFromResourceAssembler.ToCommandFromResource(createReportResource);
            var report = await reportCommandService.Handle(command);
            if (report is null) return BadRequest(new { Error = "Failed to create report" });
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
    /// <param name="userId">User ID</param>
    /// <returns>List of user reports</returns>
    /// <response code="200">Reports successfully retrieved</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">User not found or no reports</response>
    [HttpGet("get-by-user-id/{userId:int}")]
    [ProducesResponseType(typeof(ReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
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
    /// <param name="localId">Local ID</param>
    /// <returns>List of local reports</returns>
    /// <response code="200">Reports successfully retrieved</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">Local not found or no reports</response>
    [HttpGet("get-by-local-id/{localId:int}")]
    [ProducesResponseType(typeof(ReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
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
    /// <param name="reportId">ID of the report to delete</param>
    /// <returns>Confirmation message</returns>
    /// <response code="200">Report successfully deleted</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">Report not found</response>
    /// <response code="500">Internal server error</response>
    [HttpDelete("{reportId:int}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteReport(int reportId)
    {
        try
        {
            var command = new DeleteReportCommand(reportId);
            var reportDeleted = await reportCommandService.Handle(command);
            if (reportDeleted is null) return NotFound(new { Error = "Report not found or failed to delete" });
            return Ok("Report deleted");
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
}