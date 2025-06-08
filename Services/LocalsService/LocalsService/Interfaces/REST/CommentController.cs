using System.Net.Mime;
using LocalsService.Domain.Model.Queries;
using LocalsService.Domain.Services;
using LocalsService.Interfaces.REST.Resources;
using LocalsService.Interfaces.REST.Transform;
using LocalsService.Locals.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.REST.Resources;

namespace LocalsService.Interfaces.REST;

/// <summary>
/// Controlador para gestión de comentarios de locales
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class CommentController(ICommentCommandService commandService, ICommentQueryService queryService) : ControllerBase
{
    /// <summary>
    /// Endpoint GET para obtener todos los comentarios de un local específico
    /// </summary>
    /// <param name="localId">ID del local</param>
    /// <returns>Lista de comentarios del local</returns>
    /// <response code="200">Comentarios obtenidos exitosamente</response>
    /// <response code="404">Local no encontrado o sin comentarios</response>
    [HttpGet("local/{localId:int}")]
    [ProducesResponseType(typeof(CommentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllCommentsByLocalId(int localId)
    {
        try
        {
            // Crea la query con el ID del local
            var getAllCommentsByLocalIdQuery = new GetAllCommentsByLocalIdQuery(localId);
            // Ejecuta la consulta para obtener los comentarios
            var comments = await queryService.Handle(getAllCommentsByLocalIdQuery);
            // Convierte cada entidad comentario a recurso de respuesta
            var commentsResources = comments.Select(CommentResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(commentsResources);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentran comentarios o falla la consulta
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// Endpoint POST para crear un nuevo comentario
    /// </summary>
    /// <param name="resource">Datos del comentario a crear</param>
    /// <returns>Comentario creado</returns>
    /// <response code="201">Comentario creado exitosamente</response>
    /// <response code="400">Datos de entrada inválidos</response>
    /// <response code="404">Recurso relacionado no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost]
    [ProducesResponseType(typeof(CommentResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateComment(CreateCommentResource resource)
    {
        try
        {
            // Convierte el recurso de entrada en comando de creación
            var createCommentCommand = CreateCommentCommandFromResourceAssembler.ToCommandFromResource(resource);
            // Ejecuta el comando para crear el comentario
            var comment = await commandService.Handle(createCommentCommand);
            // Valida que el comentario se creó correctamente
            if (comment is null) return BadRequest();
            // Convierte la entidad creada en recurso de respuesta
            var commentResource = CommentResourceFromEntityAssembler.ToResourceFromEntity(comment);
            return StatusCode(201, commentResource);
        }
        catch (BadHttpRequestException ex)
        {
            // Maneja errores de validación de datos de entrada
            return BadRequest(new { Error = ex.Message }); // 400
        }
        catch (KeyNotFoundException ex)
        {
            // Maneja casos donde no se encuentra el recurso relacionado
            return NotFound(new { Error = ex.Message }); // 404
        }
        catch (Exception ex)
        {
            // Maneja errores generales del sistema
            return StatusCode(500, new { Error = "Internal server error", Details = ex.Message }); // 500
        }
    }
}