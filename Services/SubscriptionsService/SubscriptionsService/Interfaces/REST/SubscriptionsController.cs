using Microsoft.AspNetCore.Mvc;
using ProfilesService.Domain.Model.Queries;
using SubscriptionsService.Domain.Model.Commands;
using SubscriptionsService.Domain.Model.Queries;
using SubscriptionsService.Domain.Services;
using SubscriptionsService.Interfaces.REST.Resources;
using SubscriptionsService.Interfaces.REST.Transform;

namespace SubscriptionsService.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class SubscriptionsController(
    ISubscriptionCommandService subscriptionCommandService,
    ISubscriptionQueryServices subscriptionQueryServices)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateSubscription(
        [FromBody] CreateSubscriptionResource createSubscriptionResource)
    {
        var createSubscriptionCommand =
            CreateSubscriptionCommandFromResourceAssembler.ToCommandFromResource(createSubscriptionResource);
        var subscription = await subscriptionCommandService.Handle(createSubscriptionCommand);
        if (subscription is null) return BadRequest();
        var resource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription);
        
        return StatusCode(201, resource);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllSubscriptions()
    {
        var getAllSubscriptionsQuery = new GetAllSubscriptionsQuery();
        var subscriptions = await subscriptionQueryServices.Handle(getAllSubscriptionsQuery);
        var resources = subscriptions.Select(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    [HttpGet("status/{userId}")]
    public async Task<IActionResult> GetSubscriptionStatusByUserId([FromRoute] int userId)
    {
        var getSubscriptionStatusByUserIdQuery = new GetSubscriptionStatusByUserIdQuery(userId);
        var subscriptionStatus = await subscriptionQueryServices.Handle(getSubscriptionStatusByUserIdQuery);
        return Ok(subscriptionStatus);
    }

    [HttpPut("{subscriptionId}")]
    public async Task<IActionResult> ActiveSubscriptionStatus(int subscriptionId)
    {
        var activeSubscriptionStatusCommand = new ActiveSubscriptionStatusCommand(subscriptionId);
        var subscription = await subscriptionCommandService.Handle(activeSubscriptionStatusCommand);
        if (subscription == null) return NotFound();
        var resource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription);
        return Ok(resource);
    }
   
}