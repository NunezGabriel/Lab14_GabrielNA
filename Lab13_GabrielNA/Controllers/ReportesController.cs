using Lab13_GabrielNA.Application.Dtos;
using Lab13_GabrielNA.Application.Features.Reportes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lab13_GabrielNA.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("ventas")]
    [ProducesResponseType(typeof(IEnumerable<PedidoReporteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetVentas([FromQuery] int? clienteId, [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
    {
        if (desde > hasta)
            return BadRequest(new { mensaje = "La fecha 'desde' no puede ser mayor que la fecha 'hasta'." });

        return Ok(await _mediator.Send(new GetVentasQuery { ClienteId = clienteId, Desde = desde, Hasta = hasta }));
    }

    [HttpGet("ventas/excel")]
    [Produces(ArchivoExcelDto.TipoContenido, "application/json")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK, ArchivoExcelDto.TipoContenido)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExportVentas([FromQuery] int? clienteId, [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
    {
        if (desde > hasta)
            return BadRequest(new { mensaje = "La fecha 'desde' no puede ser mayor que la fecha 'hasta'." });

        var archivo = await _mediator.Send(new ExportarVentasExcelQuery { ClienteId = clienteId, Desde = desde, Hasta = hasta });
        if (archivo is null)
            return NotFound(new { mensaje = "No hay pedidos que coincidan con los filtros indicados." });

        return File(archivo.Contenido, ArchivoExcelDto.TipoContenido, archivo.NombreArchivo);
    }

    [HttpGet("productos")]
    [ProducesResponseType(typeof(IEnumerable<ProductoVendidoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductos([FromQuery] bool soloVendidos = false)
    {
        return Ok(await _mediator.Send(new GetProductosVendidosQuery { SoloVendidos = soloVendidos }));
    }

    [HttpGet("productos/excel")]
    [Produces(ArchivoExcelDto.TipoContenido, "application/json")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK, ArchivoExcelDto.TipoContenido)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExportProductos([FromQuery] bool soloVendidos = false)
    {
        var archivo = await _mediator.Send(new ExportarProductosExcelQuery { SoloVendidos = soloVendidos });
        if (archivo is null)
            return NotFound(new { mensaje = "No hay productos para generar el reporte." });

        return File(archivo.Contenido, ArchivoExcelDto.TipoContenido, archivo.NombreArchivo);
    }
}
