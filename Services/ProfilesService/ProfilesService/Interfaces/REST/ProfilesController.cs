using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ProfilesService.Domain.Model.Aggregates;
using ProfilesService.Domain.Model.Queries;
using ProfilesService.Domain.Services;
using ProfilesService.Interfaces.REST.Resources;
using ProfilesService.Interfaces.REST.Transform;
using Shared.Interfaces.REST.Resources;

namespace ProfilesService.Interfaces.REST;

/// <summary>
/// Controller for user profile management
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ProfilesController(
    IProfileCommandService profileCommandService, 
    IProfileQueryService profileQueryService)
    : ControllerBase
{
    /// <summary>
    /// POST endpoint to create a new user profile
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProfileResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProfile(CreateProfileResource resource)
    {
        try
        {
            var createProfileCommand = CreateProfileCommandFromResourceAssembler.ToCommandFromResource(resource);
            var profile = await profileCommandService.Handle(createProfileCommand);
            if (profile is null) return BadRequest(new { Error = "Failed to create profile" });
            var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
            return StatusCode(201, profileResource);
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
    /// GET endpoint to retrieve a specific user's profile
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(ProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileByUserId(int userId)
    {
        try
        {
            var profile = await profileQueryService.Handle(new GetProfileByUserIdQuery(userId));
            if (profile == null) return NotFound(new { Error = "Profile not found" });
            var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
            return Ok(profileResource);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// GET endpoint to retrieve bank accounts for a specific user
    /// </summary>
    [HttpGet("bank-accounts/{userId}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileBankAccountsByUserId(int userId)
    {
        try
        {
            var query = new GetProfileBankAccountsByUserIdQuery(userId);
            var bankAccounts = await profileQueryService.Handle(query);
            return Ok(bankAccounts);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// PUT endpoint to update an existing user profile
    /// </summary>
    [HttpPut("{userId}")]
    [ProducesResponseType(typeof(ProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateProfile(int userId, [FromBody] UpdateProfileResource updateProfileResource)
    {
        try
        {
            var updateProfileCommand = UpdateProfileCommandFromResourceAssembler.ToCommandFromResource(userId, updateProfileResource);
            var result = await profileCommandService.Handle(updateProfileCommand);
            if (result is null) return NotFound(new { Error = "Profile not found or failed to update" });
            return Ok(ProfileResourceFromEntityAssembler.ToResourceFromEntity(result));
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