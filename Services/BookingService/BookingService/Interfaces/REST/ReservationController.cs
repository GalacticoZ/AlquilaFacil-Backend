// ReservationController.cs
using System.Net.Mime;
using BookingService.Application.External.OutboundServices;
using BookingService.Domain.Services;
using BookingService.Domain.Model.Queries;
using BookingService.Interfaces.REST.Resources;
using BookingService.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.REST.Resources;

namespace BookingService.Interfaces.REST;

/// <summary>
/// Controlador para gestión de reservaciones
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ReservationController(IReservationCommandService reservationCommandService, IReservationQueryService reservationQueryService) : ControllerBase
{
    /// <summary>
    /// Endpoint POST para crear una nueva reservación
    /// </summary>
    /// <param name="resource">Datos de la reservación a crear</param>
    /// <returns>Reservación creada</returns>
    /// <response code="201">Reservación creada exitosamente</response>
    /// <response code="400">Datos de entrada inválidos</response>
    /// <response code="404">Recurso relacionado no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReservationResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateReservationAsync([FromBody]CreateReservationResource resource)
    {
        try
        {
            var command = CreateReservationCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await reservationCommandService.Handle(command);
            if (result is null) return BadRequest(new { Error = "Failed to create reservation" });
            var reservationResource = ReservationResourceFromEntityAssembler.ToResourceFromEntity(result);
            return StatusCode(201, reservationResource);
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
    /// Endpoint PUT para actualizar una reservación existente
    /// </summary>
    /// <param name="id">ID de la reservación a actualizar</param>
    /// <param name="resource">Datos actualizados de la reservación</param>
    /// <returns>Reservación actualizada</returns>
    /// <response code="200">Reservación actualizada exitosamente</response>
    /// <response code="400">Datos de entrada inválidos</response>
    /// <response code="404">Reservación no encontrada</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ReservationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateReservationAsync(int id, [FromBody]UpdateReservationResource resource)
    {
        try
        {
            var command = UpdateReservationDateCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await reservationCommandService.Handle(command);
            if (result is null) return NotFound(new { Error = "Reservation not found or failed to update" });
            var reservationResource = ReservationResourceFromEntityAssembler.ToResourceFromEntity(result);
            return Ok(reservationResource);
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
    /// Endpoint DELETE para eliminar una reservación
    /// </summary>
    /// <param name="id">ID de la reservación a eliminar</param>
    /// <returns>Mensaje de confirmación</returns>
    /// <response code="200">Reservación eliminada exitosamente</response>
    /// <response code="400">Datos de entrada inválidos</response>
    /// <response code="404">Reservación no encontrada</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteReservationAsync(int id)
    {
        try
        {
            var resource = new DeleteReservationResource(id);
            var command = DeleteReservationCommandFromResourceAssembler.ToCommandFromResource(resource);
            await reservationCommandService.Handle(command);
            return StatusCode(200, "Reservation deleted");
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
    /// Endpoint GET para obtener reservaciones por ID de usuario
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <returns>Lista de reservaciones del usuario</returns>
    /// <response code="200">Reservaciones obtenidas exitosamente</response>
    /// <response code="404">Usuario no encontrado o sin reservaciones</response>
    [HttpGet("by-user-id/{userId:int}")]
    [ProducesResponseType(typeof(ReservationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReservationsByUserIdAsync(int userId)
    {
        try
        {
            var query = new GetReservationsByUserId(userId);
            var result = await reservationQueryService.GetReservationsByUserIdAsync(query);
            var reservationResource = result.Select(ReservationResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(reservationResource);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// Endpoint GET para obtener detalles de reservaciones por ID de propietario
    /// </summary>
    /// <param name="userId">ID del propietario</param>
    /// <returns>Detalles de reservaciones del propietario</returns>
    /// <response code="200">Detalles obtenidos exitosamente</response>
    /// <response code="404">Propietario no encontrado o sin reservaciones</response>
    [HttpGet("reservation-user-details/{userId:int}")]
    [ProducesResponseType(typeof(ReservationDetailsResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReservationUserDetailsAsync(int userId)
    {
        try
        {
            var query = new GetReservationsByOwnerIdQuery(userId);
            var reservations = await reservationQueryService.GetReservationsByOwnerIdAsync(query);
            var reservationDetailsResource = new ReservationDetailsResource(reservations);
            return Ok(reservationDetailsResource);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// Endpoint GET para obtener reservaciones por fecha de inicio
    /// </summary>
    /// <param name="startDate">Fecha de inicio de la reservación</param>
    /// <returns>Lista de reservaciones con la fecha de inicio especificada</returns>
    /// <response code="200">Reservaciones obtenidas exitosamente</response>
    /// <response code="404">No se encontraron reservaciones para la fecha</response>
    [HttpGet("by-start-date/{startDate}")]
    [ProducesResponseType(typeof(ReservationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReservationByStartDateAsync(DateTime startDate)
    {
        try
        {
            var query = new GetReservationByStartDate(startDate);
            var result = await reservationQueryService.GetReservationByStartDateAsync(query);
            var reservationResource = result.Select(ReservationResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(reservationResource);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// Endpoint GET para obtener reservaciones por fecha de fin
    /// </summary>
    /// <param name="endDate">Fecha de fin de la reservación</param>
    /// <returns>Lista de reservaciones con la fecha de fin especificada</returns>
    /// <response code="200">Reservaciones obtenidas exitosamente</response>
    /// <response code="404">No se encontraron reservaciones para la fecha</response>
    [HttpGet("by-end-date/{endDate}")]
    [ProducesResponseType(typeof(ReservationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReservationByEndDateAsync(DateTime endDate)
    {
        try
        {
            var query = new GetReservationByEndDate(endDate);
            var result = await reservationQueryService.GetReservationByEndDateAsync(query);
            var reservationResource = result.Select(ReservationResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(reservationResource);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
}