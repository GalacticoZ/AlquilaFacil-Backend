using System.Net.Mime;
using LocalsService.Domain.Model.Queries;
using LocalsService.Domain.Services;
using LocalsService.Interfaces.REST.Resources;
using LocalsService.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.REST.Resources;

namespace LocalsService.Locals.Interfaces.REST;

/// <summary>
/// Controlador para gestión de categorías de locales
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class LocalCategoriesController(ILocalCategoryQueryService localCategoryQueryService)
    : ControllerBase
{
    /// <summary>
    /// Endpoint GET para obtener todas las categorías de locales disponibles
    /// </summary>
    /// <returns>Lista de todas las categorías de locales</returns>
    /// <response code="200">Categorías obtenidas exitosamente</response>
    /// <response code="404">No se encontraron categorías o error en la consulta</response>
    [HttpGet]
    [ProducesResponseType(typeof(LocalCategoryResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllLocalCategories()
    {
        try
        {
            // Crea la query para obtener todas las categorías de locales
            var getAllLocalCategoriesQuery = new GetAllLocalCategoriesQuery();
            // Ejecuta la consulta para obtener las categorías
            var localCategories = await localCategoryQueryService.Handle(getAllLocalCategoriesQuery);
            // Convierte cada entidad categoría a recurso de respuesta
            var localCategoryResources = localCategories.Select(LocalCategoryResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(localCategoryResources);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentran categorías o falla la consulta
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
}