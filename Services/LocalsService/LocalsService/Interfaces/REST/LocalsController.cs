using System.Net.Mime;
using LocalsService.Domain.Model.Queries;
using LocalsService.Domain.Services;
using LocalsService.Interfaces.REST.Resources;
using LocalsService.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shared.Interfaces.REST.Resources;

namespace LocalsService.Interfaces.REST;

/// <summary>
/// Controller for locals management
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
public class LocalsController(ILocalCommandService localCommandService, ILocalQueryService localQueryService)
    : ControllerBase
{
    /// <summary>
    /// POST endpoint to create a new local
    /// </summary>
    /// <param name="resource">Data of the local to create</param>
    /// <returns>Created local</returns>
    /// <response code="201">Local successfully created</response>
    /// <response code="400">Invalid input data</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">Related resource not found</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateLocal([FromBody] CreateLocalResource resource)
    {
        try
        {
            var createLocalCommand = CreateLocalCommandFromResourceAssembler.ToCommandFromResources(resource);
            var local = await localCommandService.Handle(createLocalCommand);
            if (local is null) return BadRequest(new { Error = "Failed to create local" });
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

    /// <summary>
    /// GET endpoint to retrieve all available locals
    /// </summary>
    /// <returns>List of all locals</returns>
    /// <response code="200">Locals successfully retrieved</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">No locals found</response>
    [HttpGet]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
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

    /// <summary>
    /// GET endpoint to retrieve a specific local by its ID
    /// </summary>
    /// <param name="localId">ID of the local</param>
    /// <returns>Local details</returns>
    /// <response code="200">Local successfully retrieved</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">Local not found</response>
    [HttpGet("{localId:int}")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLocalById(int localId)
    {
        try
        {
            var getLocalByIdQuery = new GetLocalByIdQuery(localId);
            var local = await localQueryService.Handle(getLocalByIdQuery);
            if (local == null) return NotFound(new { Error = "Local not found" });
            var localResource = LocalResourceFromEntityAssembler.ToResourceFromEntity(local);
            return Ok(localResource);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// PUT endpoint to update an existing local
    /// </summary>
    /// <param name="localId">ID of the local to update</param>
    /// <param name="resource">Updated local data</param>
    /// <returns>Updated local</returns>
    /// <response code="200">Local successfully updated</response>
    /// <response code="400">Invalid input data</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">Local not found</response>
    /// <response code="500">Internal server error</response>
    [HttpPut("{localId:int}")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateLocal(int localId, [FromBody] UpdateLocalResource resource)
    {
        try
        {
            var updateLocalCommand = UpdateLocalCommandFromResourceAssembler.ToCommandFromResource(localId, resource);
            var local = await localCommandService.Handle(updateLocalCommand);
            if (local is null) return NotFound(new { Error = "Local not found or failed to update" });
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

    /// <summary>
    /// GET endpoint to search locals by category and capacity range
    /// </summary>
    /// <param name="categoryId">Category ID</param>
    /// <param name="minCapacity">Minimum capacity</param>
    /// <param name="maxCapacity">Maximum capacity</param>
    /// <returns>List of locals matching the criteria</returns>
    /// <response code="200">Locals successfully retrieved</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">No locals found matching criteria</response>
    [HttpGet("search-by-category-id-capacity-range/{categoryId:int}/{minCapacity:int}/{maxCapacity:int}")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
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

    /// <summary>
    /// GET endpoint to retrieve all available districts
    /// </summary>
    /// <returns>List of all districts</returns>
    /// <response code="200">Districts successfully retrieved</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">No districts found</response>
    [HttpGet("get-all-districts")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
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

    /// <summary>
    /// GET endpoint to retrieve all locals for a specific user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>List of user's locals</returns>
    /// <response code="200">Locals successfully retrieved</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">User not found or no locals</response>
    [HttpGet("get-user-locals/{userId:int}")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
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
    
    /// <summary>
    /// GET endpoint to retrieve owner for a specific local
    /// </summary>
    /// <param name="localId">Local ID</param>
    /// <returns>Owner ID</returns>
    /// <response code="200">Owner ID successfully retrieved</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">Local not found</response>
    [HttpGet("owner/{localId:int}")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOwnerIdByLocalId(int localId)
    {
        try
        {
            var getLocalOwnerIdByLocalIdQuery = new GetLocalOwnerIdByLocalId(localId);
            var ownerId = await localQueryService.Handle(getLocalOwnerIdByLocalIdQuery);
            return Ok(ownerId);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
}