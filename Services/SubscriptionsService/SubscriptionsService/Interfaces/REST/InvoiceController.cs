using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using SubscriptionsService.Domain.Model.Queries;
using SubscriptionsService.Domain.Services;
using SubscriptionsService.Interfaces.REST.Resources;
using SubscriptionsService.Interfaces.REST.Transform;
using Shared.Interfaces.REST.Resources;

namespace SubscriptionsService.Interfaces.REST;

/// <summary>
/// Controller for invoice management
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class InvoiceController(
    IInvoiceCommandService invoiceCommandService,
    IInvoiceQueryService invoiceQueryService) 
    : ControllerBase
{
    /// <summary>
    /// POST endpoint to create a new invoice
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(InvoiceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceResource createInvoiceResource)
    {
        try
        {
            var createInvoiceCommand =
                CreateInvoiceCommandFromResourceAssembler.ToCommandFromResource(createInvoiceResource);
            var invoice = await invoiceCommandService.Handle(createInvoiceCommand);
            if (invoice is null) return BadRequest(new { Error = "Failed to create invoice" });
            var resource = InvoiceResourceFromEntityAssembler.ToResourceFromEntity(invoice);
            return Ok(resource);
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
    /// GET endpoint to retrieve all available invoices
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(InvoiceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInvoices()
    {
        try
        {
            var invoices = await invoiceQueryService.Handle(new GetAllInvoicesQuery());
            var resource = invoices.Select(InvoiceResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resource);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// GET endpoint to retrieve a specific invoice by its ID
    /// </summary>
    [HttpGet("{invoiceId}")]
    [ProducesResponseType(typeof(InvoiceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInvoiceById(int invoiceId)
    {
        try
        {
            var invoice = await invoiceQueryService.Handle(new GetInvoiceByIdQuery(invoiceId));
            if (invoice is null) return NotFound(new { Error = "Invoice not found" });
            var resource = InvoiceResourceFromEntityAssembler.ToResourceFromEntity(invoice);
            return Ok(resource);
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
}