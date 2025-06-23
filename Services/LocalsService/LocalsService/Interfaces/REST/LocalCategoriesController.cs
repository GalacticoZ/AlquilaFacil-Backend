using System.Net.Mime;
using LocalsService.Domain.Model.Queries;
using LocalsService.Domain.Services;
using LocalsService.Interfaces.REST.Resources;
using LocalsService.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shared.Interfaces.REST.Resources;

namespace LocalsService.Locals.Interfaces.REST;

/// <summary>
/// Controller for managing local categories
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
public class LocalCategoriesController(ILocalCategoryQueryService localCategoryQueryService)
    : ControllerBase
{
    /// <summary>
    /// GET endpoint to retrieve all available local categories
    /// </summary>
    /// <returns>List of all local categories</returns>
    /// <response code="200">Categories successfully retrieved</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">No categories found</response>
    [HttpGet]
    [ProducesResponseType(typeof(LocalCategoryResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllLocalCategories()
    {
        try
        {
            var getAllLocalCategoriesQuery = new GetAllLocalCategoriesQuery();
            var localCategories = await localCategoryQueryService.Handle(getAllLocalCategoriesQuery);
            var localCategoryResources = localCategories.Select(LocalCategoryResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(localCategoryResources);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
}