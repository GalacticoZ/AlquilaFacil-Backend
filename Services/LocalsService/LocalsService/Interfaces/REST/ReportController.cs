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
/// Controlador para gestión de reportes de locales
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ReportController(IReportQueryService reportQueryService, IReportCommandService reportCommandService) : ControllerBase
{
    /// <summary>
    /// Endpoint POST para crear un nuevo reporte
    /// </summary>
    /// <param name="createReportResource">Datos del reporte a crear</param>
    /// <returns>Reporte creado</returns>
    /// <response code="201">Reporte creado exitosamente</response>
    /// <response code="400">Datos de entrada inválidos</response>
    /// <response code="404">Recurso relacionado no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReportResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateReport([FromBody] CreateReportResource createReportResource)
    {
        try
        {
            // Convierte el recurso de entrada en comando de creación
            var command = CreateReportCommandFromResourceAssembler.ToCommandFromResource(createReportResource);
            // Ejecuta el comando para crear el reporte
            var report = await reportCommandService.Handle(command);
            // Convierte la entidad creada en recurso de respuesta
            var reportResource = ReportResourceFromEntityAssembler.ToResourceFromEntity(report);
            return StatusCode(201, reportResource);
        }
        catch (BadHttpRequestException ex)
        {
            // Maneja errores de validación de datos de entrada
            return BadRequest(new { Error = ex.Message }); // 400
        }
        catch (KeyNotFoundException ex)
        {
            // Maneja casos donde no se encuentra el recurso relacionado
            return NotFound(new { Error = ex.Message }); // 404
        }
        catch (Exception ex)
        {
            // Maneja errores generales del sistema
            return StatusCode(500, new { Error = "Internal server error", Details = ex.Message }); // 500
        }
    }

    /// <summary>
    /// Endpoint GET para obtener todos los reportes de un usuario específico
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <returns>Lista de reportes del usuario</returns>
    /// <response code="200">Reportes obtenidos exitosamente</response>
    /// <response code="404">Usuario no encontrado o sin reportes</response>
    [HttpGet("get-by-user-id/{userId:int}")]
    [ProducesResponseType(typeof(ReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReportsByUserId(int userId)
    {
        try
        {
            // Crea la query con el ID del usuario
            var query = new GetReportsByUserIdQuery(userId);
            // Ejecuta la consulta para obtener los reportes del usuario
            var reports = await reportQueryService.Handle(query);
            // Convierte cada entidad reporte a recurso de respuesta
            var reportResources = reports.Select(ReportResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(reportResources);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentran reportes del usuario o falla la consulta
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// Endpoint GET para obtener todos los reportes de un local específico
    /// </summary>
    /// <param name="localId">ID del local</param>
    /// <returns>Lista de reportes del local</returns>
    /// <response code="200">Reportes obtenidos exitosamente</response>
    /// <response code="404">Local no encontrado o sin reportes</response>
    [HttpGet("get-by-local-id/{localId:int}")]
    [ProducesResponseType(typeof(ReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReportsByLocalId(int localId)
    {
        try
        {
            // Crea la query con el ID del local
            var query = new GetReportsByLocalIdQuery(localId);
            // Ejecuta la consulta para obtener los reportes del local
            var reports = await reportQueryService.Handle(query);
            // Convierte cada entidad reporte a recurso de respuesta
            var reportResources = reports.Select(ReportResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(reportResources);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentran reportes del local o falla la consulta
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// Endpoint DELETE para eliminar un reporte específico
    /// </summary>
    /// <param name="reportId">ID del reporte a eliminar</param>
    /// <returns>Confirmación de eliminación</returns>
    /// <response code="200">Reporte eliminado exitosamente</response>
    /// <response code="404">Reporte no encontrado</response>
    [HttpDelete("{reportId:int}")]
    [ProducesResponseType(typeof(ReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReport(int reportId)
    {
        try
        {
            // Crea el comando con el ID del reporte a eliminar
            var command = new DeleteReportCommand(reportId);
            // Ejecuta el comando para eliminar el reporte
            var reportDeleted = await reportCommandService.Handle(command);
            return StatusCode(200, reportDeleted);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentra el reporte o falla la eliminación
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
}