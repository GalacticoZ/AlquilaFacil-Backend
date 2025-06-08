using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using SubscriptionsService.Domain.Model.Queries;
using SubscriptionsService.Domain.Services;
using SubscriptionsService.Interfaces.REST.Resources;
using SubscriptionsService.Interfaces.REST.Transform;
using Shared.Interfaces.REST.Resources;

namespace SubscriptionsService.Interfaces.REST;

/// <summary>
/// Controlador para gestión de facturas
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
    /// Endpoint POST para crear una nueva factura
    /// </summary>
    /// <param name="createInvoiceResource">Datos de la factura a crear</param>
    /// <returns>Factura creada</returns>
    /// <response code="200">Factura creada exitosamente</response>
    /// <response code="400">Datos de entrada inválidos o no se pudo crear la factura</response>
    /// <response code="404">Recurso relacionado no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost]
    [ProducesResponseType(typeof(InvoiceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceResource createInvoiceResource)
    {
        try
        {
            // Convierte el recurso de entrada en comando de creación
            var createInvoiceCommand =
                CreateInvoiceCommandFromResourceAssembler.ToCommandFromResource(createInvoiceResource);
            // Ejecuta el comando para crear la factura
            var invoice = await invoiceCommandService.Handle(createInvoiceCommand);
            // Valida que la factura se creó correctamente
            if (invoice is null) return BadRequest(new { Error = "Failed to create invoice" });
            // Convierte la entidad creada en recurso de respuesta
            var resource = InvoiceResourceFromEntityAssembler.ToResourceFromEntity(invoice);
            return Ok(resource);
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
    /// Endpoint GET para obtener todas las facturas disponibles
    /// </summary>
    /// <returns>Lista de todas las facturas</returns>
    /// <response code="200">Facturas obtenidas exitosamente</response>
    /// <response code="404">No se encontraron facturas o error en la consulta</response>
    [HttpGet]
    [ProducesResponseType(typeof(InvoiceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInvoices()
    {
        try
        {
            // Ejecuta la consulta para obtener todas las facturas
            var invoices = await invoiceQueryService.Handle(new GetAllInvoicesQuery());
            // Convierte cada entidad factura a recurso de respuesta
            var resource = invoices.Select(InvoiceResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resource);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentran facturas o falla la consulta
            return NotFound(new { Error = ex.Message }); // 404
        }
    }

    /// <summary>
    /// Endpoint GET para obtener una factura específica por su ID
    /// </summary>
    /// <param name="invoiceId">ID de la factura a buscar</param>
    /// <returns>Factura encontrada</returns>
    /// <response code="200">Factura obtenida exitosamente</response>
    /// <response code="404">Factura no encontrada</response>
    [HttpGet("{invoiceId}")]
    [ProducesResponseType(typeof(InvoiceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseResource), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInvoiceById(int invoiceId)
    {
        try
        {
            // Ejecuta la consulta para obtener la factura específica
            var invoice = await invoiceQueryService.Handle(new GetInvoiceByIdQuery(invoiceId));
            // Valida que la factura existe
            if (invoice is null) return NotFound(new { Error = "Invoice not found" });
            // Convierte la entidad factura a recurso de respuesta
            var resource = InvoiceResourceFromEntityAssembler.ToResourceFromEntity(invoice);
            return Ok(resource);
        }
        catch (Exception ex)
        {
            // Maneja error cuando no se encuentra la factura o falla la consulta
            return NotFound(new { Error = ex.Message }); // 404
        }
    }
}