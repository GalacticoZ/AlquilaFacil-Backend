using System.Net.Mime;
using LocalsService.Domain.Model.Queries;
using LocalsService.Domain.Services;
using LocalsService.Interfaces.REST.Resources;
using LocalsService.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.REST.Resources;

namespace LocalsService.Interfaces.REST;


[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class LocalsController(ILocalCommandService localCommandService, ILocalQueryService localQueryService)
:ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateLocal(CreateLocalResource resource)
    {
        try
        {
            var createLocalCommand = CreateLocalCommandFromResourceAssembler.ToCommandFromResources(resource);
            var local = await localCommandService.Handle(createLocalCommand);
            if (local is null) return BadRequest(new { error = "Could not create local" });
            var localResource = LocalResourceFromEntityAssembler.ToResourceFromEntity(local);
            return CreatedAtAction(nameof(GetLocalById), new { localId = localResource.Id }, localResource);
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

    
    [HttpGet]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllLocals()
    {
        try
        {
            var getAllLocalsQuery = new GetAllLocalsQuery();
            var locals = await localQueryService.Handle(getAllLocalsQuery);
            var localResources = locals.Select(LocalResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(localResources);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
       
    }
    

    [HttpGet("{localId:int}")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLocalById(int localId)
    {
        try
        {
            var getLocalByIdQuery = new GetLocalByIdQuery(localId);
            var local = await localQueryService.Handle(getLocalByIdQuery);
            if (local == null) return NotFound();
            var localResource = LocalResourceFromEntityAssembler.ToResourceFromEntity(local);
            return Ok(localResource);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
    
    [HttpPut("{localId:int}")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLocal(int localId, UpdateLocalResource resource)
    {
        try
        {
            var updateLocalCommand = UpdateLocalCommandFromResourceAssembler.ToCommandFromResource(localId, resource);
            var local = await localCommandService.Handle(updateLocalCommand);
            if (local is null) return BadRequest();
            var localResource = LocalResourceFromEntityAssembler.ToResourceFromEntity(local);
            return Ok(localResource);
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
    
    
    [HttpGet("search-by-category-id-capacity-range/{categoryId:int}/{minCapacity:int}/{maxCapacity:int}")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchByCategoryIdAndCapacityRange(int categoryId, int minCapacity, int maxCapacity)
    {
        try
        {
            var searchByCategoryIdAndCapacityRangeQuery = new GetLocalsByCategoryIdAndCapacityRangeQuery(categoryId, minCapacity, maxCapacity);
            var locals = await localQueryService.Handle(searchByCategoryIdAndCapacityRangeQuery);
            var localResources = locals.Select(LocalResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(localResources);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
    
    [HttpGet("get-all-districts")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public IActionResult GetAllDistricts()
    {
        try
        {
            var getAllLocalDistrictsQuery = new GetAllLocalDistrictsQuery();
            var districts = localQueryService.Handle(getAllLocalDistrictsQuery);
            return Ok(districts);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
    
    [HttpGet("get-user-locals/{userId:int}")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserLocals(int userId)
    {
        try
        {
            var getUserLocalsQuery = new GetLocalsByUserIdQuery(userId);
            var locals = await localQueryService.Handle(getUserLocalsQuery);
            var localResources = locals.Select(LocalResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(localResources);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
    
}