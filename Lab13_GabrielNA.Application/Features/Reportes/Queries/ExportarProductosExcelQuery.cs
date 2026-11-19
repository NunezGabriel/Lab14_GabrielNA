using Lab13_GabrielNA.Application.Dtos;
using Lab13_GabrielNA.Application.Ports.Reports;
using Lab13_GabrielNA.Application.Ports.Repositories;
using MediatR;

namespace Lab13_GabrielNA.Application.Features.Reportes.Queries;

public record ExportarProductosExcelQuery : IRequest<ArchivoExcelDto?>
{
    public bool SoloVendidos { get; set; }
}

internal sealed class ExportarProductosExcelQueryHandler : IRequestHandler<ExportarProductosExcelQuery, ArchivoExcelDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExcelReportGenerator _excel;

    public ExportarProductosExcelQueryHandler(IUnitOfWork unitOfWork, IExcelReportGenerator excel)
    {
        _unitOfWork = unitOfWork;
        _excel = excel;
    }

    public async Task<ArchivoExcelDto?> Handle(ExportarProductosExcelQuery request, CancellationToken cancellationToken)
    {
        var productos = (await _unitOfWork.Products.ObtenerProductosVendidosAsync(request.SoloVendidos)).ToList();
        if (productos.Count == 0)
            return null;

        var detalle = await _unitOfWork.Products.ObtenerDetalleVentasAsync(null);
        var filtros = request.SoloVendidos ? "Solo productos con ventas" : "Todos los productos";

        return new ArchivoExcelDto
        {
            Contenido     = _excel.GenerarReporteProductos(productos, detalle, filtros),
            NombreArchivo = $"ReporteProductos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
        };
    }
}
