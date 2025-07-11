using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProfilesService.Domain.Model.Queries;
using SubscriptionsService.Domain.Model.Commands;
using SubscriptionsService.Domain.Model.Queries;
using SubscriptionsService.Domain.Services;
using SubscriptionsService.Interfaces.REST.Resources;
using SubscriptionsService.Interfaces.REST.Transform;
using Shared.Interfaces.REST.Resources;

namespace SubscriptionsService.Interfaces.REST;

/// <summary>
/// Controller for subscription management
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
public class SubscriptionsController(
    ISubscriptionCommandService subscriptionCommandService,
    ISubscriptionQueryServices subscriptionQueryServices)
    : ControllerBase
{
    /// <summary>
    /// POST endpoint to create a new subscription
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateSubscription(
        [FromBody] CreateSubscriptionResource createSubscriptionResource)
    {
        try
        {
            var createSubscriptionCommand =
                CreateSubscriptionCommandFromResourceAssembler.ToCommandFromResource(createSubscriptionResource);
            var subscription = await subscriptionCommandService.Handle(createSubscriptionCommand);
            if (subscription is null) return BadRequest(new { Error = "Failed to create subscription" });
            var resource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription);
            return StatusCode(201, resource);
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
    /// GET endpoint to get all available subscriptions
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllSubscriptions()
    {
        try
        {
            var getAllSubscriptionsQuery = new GetAllSubscriptionsQuery();
            var subscriptions = await subscriptionQueryServices.Handle(getAllSubscriptionsQuery);
            var resources = subscriptions.Select(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// GET endpoint to get the subscription status of a specific user
    /// </summary>
    [HttpGet("status/{userId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubscriptionStatusByUserId([FromRoute] int userId)
    {
        try
        {
            var getSubscriptionStatusByUserIdQuery = new GetSubscriptionStatusByUserIdQuery(userId);
            var subscriptionStatus = await subscriptionQueryServices.Handle(getSubscriptionStatusByUserIdQuery);
            return Ok(subscriptionStatus);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// PUT endpoint to activate the status of a subscription
    /// </summary>
    [HttpPut("{subscriptionId}")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActiveSubscriptionStatus(int subscriptionId)
    {
        try
        {
            var activeSubscriptionStatusCommand = new ActiveSubscriptionStatusCommand(subscriptionId);
            var subscription = await subscriptionCommandService.Handle(activeSubscriptionStatusCommand);
            if (subscription == null) return NotFound(new { Error = "Subscription not found" });
            var resource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription);
            return Ok(resource);
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
}