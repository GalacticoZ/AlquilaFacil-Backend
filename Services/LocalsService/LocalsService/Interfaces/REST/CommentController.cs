using System.Net.Mime;
using LocalsService.Domain.Model.Queries;
using LocalsService.Domain.Services;
using LocalsService.Interfaces.REST.Resources;
using LocalsService.Interfaces.REST.Transform;
using LocalsService.Locals.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shared.Interfaces.REST.Resources;

namespace LocalsService.Interfaces.REST;

/// <summary>
/// Controller for managing local comments
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
public class CommentController(ICommentCommandService commandService, ICommentQueryService queryService) : ControllerBase
{
    /// <summary>
    /// GET endpoint to retrieve all comments for a specific local
    /// </summary>
    /// <param name="localId">ID of the local</param>
    /// <returns>List of comments for the local</returns>
    /// <response code="200">Comments successfully retrieved</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">Local not found or no comments</response>
    [HttpGet("local/{localId:int}")]
    [ProducesResponseType(typeof(CommentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllCommentsByLocalId(int localId)
    {
        try
        {
            var getAllCommentsByLocalIdQuery = new GetAllCommentsByLocalIdQuery(localId);
            var comments = await queryService.Handle(getAllCommentsByLocalIdQuery);
            var commentsResources = comments.Select(CommentResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(commentsResources);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// POST endpoint to create a new comment
    /// </summary>
    /// <param name="resource">Data of the comment to create</param>
    /// <returns>Created comment</returns>
    /// <response code="201">Comment successfully created</response>
    /// <response code="400">Invalid input data</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">Related resource not found</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(CommentResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentResource resource)
    {
        try
        {
            var createCommentCommand = CreateCommentCommandFromResourceAssembler.ToCommandFromResource(resource);
            var comment = await commandService.Handle(createCommentCommand);
            if (comment is null) return BadRequest(new { Error = "Failed to create comment" });
            var commentResource = CommentResourceFromEntityAssembler.ToResourceFromEntity(comment);
            return StatusCode(201, commentResource);
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