using System.Net.Mime;
using LocalsService.Domain.Model.Queries;
using LocalsService.Domain.Services;
using LocalsService.Interfaces.REST.Resources;
using LocalsService.Interfaces.REST.Transform;
using LocalsService.Locals.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace LocalsService.Interfaces.REST;


[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class CommentController(ICommentCommandService commandService, ICommentQueryService queryService) : ControllerBase
{
    [HttpGet("local/{localId:int}")]
    public async Task<IActionResult> GetAllCommentsByLocalId(int localId)
    {
        var getAllCommentsByLocalIdQuery = new GetAllCommentsByLocalIdQuery(localId);
        var comments = await queryService.Handle(getAllCommentsByLocalIdQuery);
        var commentsResources = comments.Select(CommentResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(commentsResources);
    }

    [HttpPost]
    public async Task<IActionResult> CreateComment(CreateCommentResource resource)
    {
        var createCommentCommand = CreateCommentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var comment = await commandService.Handle(createCommentCommand);
        if (comment is null) return BadRequest();
        var commentResource = CommentResourceFromEntityAssembler.ToResourceFromEntity(comment);
        return StatusCode(201,commentResource);
    }
}