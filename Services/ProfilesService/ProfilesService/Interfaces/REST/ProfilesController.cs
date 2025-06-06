using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ProfilesService.Domain.Model.Aggregates;
using ProfilesService.Domain.Model.Queries;
using ProfilesService.Domain.Services;
using ProfilesService.Interfaces.REST.Resources;
using ProfilesService.Interfaces.REST.Transform;

namespace ProfilesService.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ProfilesController(
    IProfileCommandService profileCommandService, 
    IProfileQueryService profileQueryService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProfile(CreateProfileResource resource)
    {
        var createProfileCommand = CreateProfileCommandFromResourceAssembler.ToCommandFromResource(resource);
        var profile = await profileCommandService.Handle(createProfileCommand);
        if (profile is null) return BadRequest();
        var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return StatusCode(201, profileResource);
    }
    
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetProfileByUserId(int userId)
    {
        var profile = await profileQueryService.Handle(new GetProfileByUserIdQuery(userId));
        if (profile == null) return NotFound();
        var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(profileResource);
    }
    
        
    [HttpGet("bank-accounts/{userId}")]
    public async Task<IActionResult> GetProfileBankAccountsByUserId(int userId)
    {
        var query = new GetProfileBankAccountsByUserIdQuery(userId);
        var bankAccounts = await profileQueryService.Handle(query);
        return Ok(bankAccounts);
    }
    
    [HttpPut("{userId}")]
    public async Task<ActionResult> UpdateProfile(int userId, [FromBody] UpdateProfileResource updateProfileResource)
    {
        var updateProfileCommand = UpdateProfileCommandFromResourceAssembler.ToCommandFromResource(userId, updateProfileResource);
        var result = await profileCommandService.Handle(updateProfileCommand);
        return Ok(ProfileResourceFromEntityAssembler.ToResourceFromEntity(result));
    }

}