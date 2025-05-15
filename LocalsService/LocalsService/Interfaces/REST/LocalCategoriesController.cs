using System.Net.Mime;
using LocalsService.Domain.Model.Queries;
using LocalsService.Domain.Services;
using LocalsService.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LocalsService.Locals.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class LocalCategoriesController(ILocalCategoryQueryService localCategoryQueryService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllLocalCategories()
    {
        var getAllLocalCategoriesQuery = new GetAllLocalCategoriesQuery();
        var localCategories = await localCategoryQueryService.Handle(getAllLocalCategoriesQuery);
        var localCategoryResources = localCategories.Select(LocalCategoryResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(localCategoryResources);
    }
}
