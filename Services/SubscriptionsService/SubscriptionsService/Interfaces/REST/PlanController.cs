using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using SubscriptionsService.Domain.Model.Queries;
using SubscriptionsService.Domain.Services;
using SubscriptionsService.Interfaces.REST.Resources;
using SubscriptionsService.Interfaces.REST.Transform;
using Shared.Interfaces.REST.Resources;

namespace SubscriptionsService.Interfaces.REST;

/// <summary>
/// Controlador para gestión de planes de suscripción
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class PlanController(IPlanQueryService planQueryService) : ControllerBase
{
    /// <summary>
    /// Endpoint GET para obtener todos los planes de suscripción disponibles
    /// </summary>
    /// <returns>Lista de todos los planes</returns>
    /// <response code="200">Planes obtenidos exitosamente</response>
    /// <response code="404">No se encontraron planes o error en la consulta</response>
    [HttpGet]
    [ProducesResponseType(typeof(PlanResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllPlans()
    {
        try
        {
            // Crea la query para obtener todos los planes
            var getAllPlansQuery = new GetAllPlansQuery();
            // Ejecuta la consulta para obtener los planes
            var plans = await planQueryService.Handle(getAllPlansQuery);
            // Convierte cada entidad plan a recurso de respuesta
            var resources = plans.Select(PlanResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentran planes o falla la consulta
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
}