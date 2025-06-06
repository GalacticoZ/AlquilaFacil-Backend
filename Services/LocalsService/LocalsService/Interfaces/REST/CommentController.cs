using System.Net.Mime;
using LocalsService.Domain.Model.Queries;
using LocalsService.Domain.Services;
using LocalsService.Interfaces.REST.Resources;
using LocalsService.Interfaces.REST.Transform;
using LocalsService.Locals.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.REST.Resources;

namespace LocalsService.Interfaces.REST;


[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class CommentController(ICommentCommandService commandService, ICommentQueryService queryService) : ControllerBase
{
    [HttpGet("local/{localId:int}")]
    [ProducesResponseType(typeof(CommentResource), StatusCodes.Status200OK)]
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

    [HttpPost]
    [ProducesResponseType(typeof(CommentResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateComment(CreateCommentResource resource)
    {
        try
        {
            var createCommentCommand = CreateCommentCommandFromResourceAssembler.ToCommandFromResource(resource);
            var comment = await commandService.Handle(createCommentCommand);
            if (comment is null) return BadRequest();
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