using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using SubscriptionsService.Domain.Model.Queries;
using SubscriptionsService.Domain.Services;
using SubscriptionsService.Interfaces.REST.Resources;
using SubscriptionsService.Interfaces.REST.Transform;
using Shared.Interfaces.REST.Resources;

namespace SubscriptionsService.Interfaces.REST;

/// <summary>
/// Controller for subscription plan management
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class PlanController(IPlanQueryService planQueryService) : ControllerBase
{
    /// <summary>
    /// GET endpoint to get all available subscription plans
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PlanResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllPlans()
    {
        try
        {
            // Create the query to get all plans
            var getAllPlansQuery = new GetAllPlansQuery();
            // Execute the query to get the plans
            var plans = await planQueryService.Handle(getAllPlansQuery);
            // Convert each plan entity to a response resource
            var resources = plans.Select(PlanResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            // Handle errors when no plans are found or the query fails
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
}