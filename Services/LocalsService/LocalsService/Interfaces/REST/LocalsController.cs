using System.Net.Mime;
using LocalsService.Domain.Model.Queries;
using LocalsService.Domain.Services;
using LocalsService.Interfaces.REST.Resources;
using LocalsService.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LocalsService.Interfaces.REST;


[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class LocalsController(ILocalCommandService localCommandService, ILocalQueryService localQueryService)
:ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateLocal(CreateLocalResource resource)
    {
        var createLocalCommand = CreateLocalCommandFromResourceAssembler.ToCommandFromResources(resource);
        var local = await localCommandService.Handle(createLocalCommand);
        if (local is null) return BadRequest();
        var localResource = LocalResourceFromEntityAssembler.ToResourceFromEntity(local);
        return CreatedAtAction(nameof(GetLocalById), new { localId = localResource.Id }, localResource);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllLocals()
    {
        var getAllLocalsQuery = new GetAllLocalsQuery();
        var locals = await localQueryService.Handle(getAllLocalsQuery);
        var localResources = locals.Select(LocalResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(localResources);
    }
    

    [HttpGet("{localId:int}")]
    public async Task<IActionResult> GetLocalById(int localId)
    {
        var getLocalByIdQuery = new GetLocalByIdQuery(localId);
        var local = await localQueryService.Handle(getLocalByIdQuery);
        if (local == null) return NotFound();
        var localResource = LocalResourceFromEntityAssembler.ToResourceFromEntity(local);
        return Ok(localResource);
    }
    
    [HttpPut("{localId:int}")]
    public async Task<IActionResult> UpdateLocal(int localId, UpdateLocalResource resource)
    {
        var updateLocalCommand = UpdateLocalCommandFromResourceAssembler.ToCommandFromResource(localId, resource);
        var local = await localCommandService.Handle(updateLocalCommand);
        if (local is null) return BadRequest();
        var localResource = LocalResourceFromEntityAssembler.ToResourceFromEntity(local);
        return Ok(localResource);
    }
    
    
    [HttpGet("search-by-category-id-capacity-range/{categoryId:int}/{minCapacity:int}/{maxCapacity:int}")]
    public async Task<IActionResult> SearchByCategoryIdAndCapacityRange(int categoryId, int minCapacity, int maxCapacity)
    {
        var searchByCategoryIdAndCapacityRangeQuery = new GetLocalsByCategoryIdAndCapacityRangeQuery(categoryId, minCapacity, maxCapacity);
        var locals = await localQueryService.Handle(searchByCategoryIdAndCapacityRangeQuery);
        var localResources = locals.Select(LocalResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(localResources);
    }
    
    [HttpGet("get-all-districts")]
    public IActionResult GetAllDistricts()
    {
        var getAllLocalDistrictsQuery = new GetAllLocalDistrictsQuery();
        var districts = localQueryService.Handle(getAllLocalDistrictsQuery);
        return Ok(districts);
    }
    
    [HttpGet("get-user-locals/{userId:int}")]
    public async Task<IActionResult> GetUserLocals(int userId)
    {
        var getUserLocalsQuery = new GetLocalsByUserIdQuery(userId);
        var locals = await localQueryService.Handle(getUserLocalsQuery);
        var localResources = locals.Select(LocalResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(localResources);
    }
    
}