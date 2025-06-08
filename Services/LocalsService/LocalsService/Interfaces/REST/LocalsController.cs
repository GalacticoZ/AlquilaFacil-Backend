using System.Net.Mime;
using LocalsService.Domain.Model.Queries;
using LocalsService.Domain.Services;
using LocalsService.Interfaces.REST.Resources;
using LocalsService.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.REST.Resources;

namespace LocalsService.Interfaces.REST;

/// <summary>
/// Controlador para gestión de locales
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class LocalsController(ILocalCommandService localCommandService, ILocalQueryService localQueryService)
    : ControllerBase
{
    /// <summary>
    /// Endpoint POST para crear un nuevo local
    /// </summary>
    /// <param name="resource">Datos del local a crear</param>
    /// <returns>Local creado</returns>
    /// <response code="201">Local creado exitosamente</response>
    /// <response code="400">Datos de entrada inválidos o no se pudo crear el local</response>
    /// <response code="404">Recurso relacionado no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateLocal(CreateLocalResource resource)
    {
        try
        {
            // Convierte el recurso de entrada en comando de creación
            var createLocalCommand = CreateLocalCommandFromResourceAssembler.ToCommandFromResources(resource);
            // Ejecuta el comando para crear el local
            var local = await localCommandService.Handle(createLocalCommand);
            // Valida que el local se creó correctamente
            if (local is null) return BadRequest(new { error = "Could not create local" });
            // Convierte la entidad creada en recurso de respuesta
            var localResource = LocalResourceFromEntityAssembler.ToResourceFromEntity(local);
            return CreatedAtAction(nameof(GetLocalById), new { localId = localResource.Id }, localResource);
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

    /// <summary>
    /// Endpoint GET para obtener todos los locales disponibles
    /// </summary>
    /// <returns>Lista de todos los locales</returns>
    /// <response code="200">Locales obtenidos exitosamente</response>
    /// <response code="404">No se encontraron locales o error en la consulta</response>
    [HttpGet]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllLocals()
    {
        try
        {
            // Crea la query para obtener todos los locales
            var getAllLocalsQuery = new GetAllLocalsQuery();
            // Ejecuta la consulta para obtener los locales
            var locals = await localQueryService.Handle(getAllLocalsQuery);
            // Convierte cada entidad local a recurso de respuesta
            var localResources = locals.Select(LocalResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(localResources);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentran locales o falla la consulta
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// Endpoint GET para obtener un local específico por su ID
    /// </summary>
    /// <param name="localId">ID del local a buscar</param>
    /// <returns>Local encontrado</returns>
    /// <response code="200">Local obtenido exitosamente</response>
    /// <response code="404">Local no encontrado</response>
    [HttpGet("{localId:int}")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLocalById(int localId)
    {
        try
        {
            // Crea la query con el ID del local
            var getLocalByIdQuery = new GetLocalByIdQuery(localId);
            // Ejecuta la consulta para obtener el local específico
            var local = await localQueryService.Handle(getLocalByIdQuery);
            // Valida que el local existe
            if (local == null) return NotFound();
            // Convierte la entidad local a recurso de respuesta
            var localResource = LocalResourceFromEntityAssembler.ToResourceFromEntity(local);
            return Ok(localResource);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentra el local o falla la consulta
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// Endpoint PUT para actualizar un local existente
    /// </summary>
    /// <param name="localId">ID del local a actualizar</param>
    /// <param name="resource">Datos actualizados del local</param>
    /// <returns>Local actualizado</returns>
    /// <response code="200">Local actualizado exitosamente</response>
    /// <response code="400">Datos de entrada inválidos o no se pudo actualizar el local</response>
    /// <response code="404">Local no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPut("{localId:int}")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLocal(int localId, UpdateLocalResource resource)
    {
        try
        {
            // Convierte el recurso de entrada en comando de actualización
            var updateLocalCommand = UpdateLocalCommandFromResourceAssembler.ToCommandFromResource(localId, resource);
            // Ejecuta el comando para actualizar el local
            var local = await localCommandService.Handle(updateLocalCommand);
            // Valida que el local se actualizó correctamente
            if (local is null) return BadRequest();
            // Convierte la entidad actualizada en recurso de respuesta
            var localResource = LocalResourceFromEntityAssembler.ToResourceFromEntity(local);
            return Ok(localResource);
        }
        catch (BadHttpRequestException ex)
        {
            // Maneja errores de validación de datos de entrada
            return BadRequest(new { Error = ex.Message }); // 400
        }
        catch (KeyNotFoundException ex)
        {
            // Maneja casos donde no se encuentra el local a actualizar
            return NotFound(new { Error = ex.Message }); // 404
        }
        catch (Exception ex)
        {
            // Maneja errores generales del sistema
            return StatusCode(500, new { Error = "Internal server error", Details = ex.Message }); // 500
        }
    }

    /// <summary>
    /// Endpoint GET para buscar locales por categoría y rango de capacidad
    /// </summary>
    /// <param name="categoryId">ID de la categoría</param>
    /// <param name="minCapacity">Capacidad mínima</param>
    /// <param name="maxCapacity">Capacidad máxima</param>
    /// <returns>Lista de locales que coinciden con los criterios</returns>
    /// <response code="200">Locales encontrados exitosamente</response>
    /// <response code="404">No se encontraron locales o error en la consulta</response>
    [HttpGet("search-by-category-id-capacity-range/{categoryId:int}/{minCapacity:int}/{maxCapacity:int}")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchByCategoryIdAndCapacityRange(int categoryId, int minCapacity, int maxCapacity)
    {
        try
        {
            // Crea la query con los parámetros de búsqueda
            var searchByCategoryIdAndCapacityRangeQuery = new GetLocalsByCategoryIdAndCapacityRangeQuery(categoryId, minCapacity, maxCapacity);
            // Ejecuta la consulta para obtener los locales que coinciden
            var locals = await localQueryService.Handle(searchByCategoryIdAndCapacityRangeQuery);
            // Convierte cada entidad local a recurso de respuesta
            var localResources = locals.Select(LocalResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(localResources);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentran locales o falla la consulta
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// Endpoint GET para obtener todos los distritos disponibles
    /// </summary>
    /// <returns>Lista de todos los distritos</returns>
    /// <response code="200">Distritos obtenidos exitosamente</response>
    /// <response code="404">No se encontraron distritos o error en la consulta</response>
    [HttpGet("get-all-districts")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public IActionResult GetAllDistricts()
    {
        try
        {
            // Crea la query para obtener todos los distritos
            var getAllLocalDistrictsQuery = new GetAllLocalDistrictsQuery();
            // Ejecuta la consulta para obtener los distritos
            var districts = localQueryService.Handle(getAllLocalDistrictsQuery);
            return Ok(districts);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentran distritos o falla la consulta
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// Endpoint GET para obtener todos los locales de un usuario específico
    /// </summary>
    /// <param name="userId">ID del usuario propietario</param>
    /// <returns>Lista de locales del usuario</returns>
    /// <response code="200">Locales del usuario obtenidos exitosamente</response>
    /// <response code="404">Usuario no encontrado o sin locales</response>
    [HttpGet("get-user-locals/{userId:int}")]
    [ProducesResponseType(typeof(LocalResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserLocals(int userId)
    {
        try
        {
            // Crea la query con el ID del usuario
            var getUserLocalsQuery = new GetLocalsByUserIdQuery(userId);
            // Ejecuta la consulta para obtener los locales del usuario
            var locals = await localQueryService.Handle(getUserLocalsQuery);
            // Convierte cada entidad local a recurso de respuesta
            var localResources = locals.Select(LocalResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(localResources);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentran locales del usuario o falla la consulta
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
}