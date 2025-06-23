// ReservationController.cs
using System.Net.Mime;
using BookingService.Application.External.OutboundServices;
using BookingService.Domain.Services;
using BookingService.Domain.Model.Queries;
using BookingService.Interfaces.REST.Resources;
using BookingService.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shared.Interfaces.REST.Resources;

namespace BookingService.Interfaces.REST;

/// <summary>
/// Controller for reservation management
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize] 
public class ReservationController(IReservationCommandService reservationCommandService, IReservationQueryService reservationQueryService) : ControllerBase
{
    /// <summary>
    /// POST endpoint to create a new reservation
    /// </summary>
    /// <param name="resource">Data of the reservation to create</param>
    /// <returns>Created reservation</returns>
    /// <response code="201">Reservation successfully created</response>
    /// <response code="400">Invalid input data</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">Related resource not found</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReservationResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateReservationAsync([FromBody]CreateReservationResource resource)
    {
        try
        {
            // 🔑 Puedes obtener el userId del token JWT si lo necesitas
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
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
    /// PUT endpoint to update an existing reservation
    /// </summary>
    /// <param name="id">ID of the reservation to update</param>
    /// <param name="resource">Updated reservation data</param>
    /// <returns>Updated reservation</returns>
    /// <response code="200">Reservation successfully updated</response>
    /// <response code="400">Invalid input data</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">Reservation not found</response>
    /// <response code="500">Internal server error</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ReservationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
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
    /// DELETE endpoint to delete a reservation
    /// </summary>
    /// <param name="id">ID of the reservation to delete</param>
    /// <returns>Confirmation message</returns>
    /// <response code="200">Reservation successfully deleted</response>
    /// <response code="400">Invalid input data</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">Reservation not found</response>
    /// <response code="500">Internal server error</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
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
    /// GET endpoint to retrieve reservations by user ID
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>List of user reservations</returns>
    /// <response code="200">Reservations successfully retrieved</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">User not found or no reservations</response>
    [HttpGet("by-user-id/{userId:int}")]
    [ProducesResponseType(typeof(ReservationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
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
    /// GET endpoint to retrieve reservation details by owner ID
    /// </summary>
    /// <param name="userId">Owner ID</param>
    /// <returns>Reservation details for the owner</returns>
    /// <response code="200">Details successfully retrieved</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">Owner not found or no reservations</response>
    [HttpGet("reservation-user-details/{userId:int}")]
    [ProducesResponseType(typeof(ReservationDetailsResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
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
    /// GET endpoint to retrieve reservations by start date
    /// </summary>
    /// <param name="startDate">Start date of the reservation</param>
    /// <returns>List of reservations with the specified start date</returns>
    /// <response code="200">Reservations successfully retrieved</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">No reservations found for the date</response>
    [HttpGet("by-start-date/{startDate}")]
    [ProducesResponseType(typeof(ReservationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
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
    /// GET endpoint to retrieve reservations by end date
    /// </summary>
    /// <param name="endDate">End date of the reservation</param>
    /// <returns>List of reservations with the specified end date</returns>
    /// <response code="200">Reservations successfully retrieved</response>
    /// <response code="401">Unauthorized - Token required</response>
    /// <response code="404">No reservations found for the date</response>
    [HttpGet("by-end-date/{endDate}")]
    [ProducesResponseType(typeof(ReservationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status401Unauthorized)]
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