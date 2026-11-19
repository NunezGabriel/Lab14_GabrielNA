using Lab13_GabrielNA.Application.Dtos;
using Lab13_GabrielNA.Application.Ports.Reports;
using Lab13_GabrielNA.Application.Ports.Repositories;
using Lab13_GabrielNA.Domain.Entities;
using MediatR;

namespace Lab13_GabrielNA.Application.Features.Reportes.Queries;

public record ExportarVentasExcelQuery : IRequest<ArchivoExcelDto?>
{
    public int? ClienteId { get; set; }
    public DateTime? Desde { get; set; }
    public DateTime? Hasta { get; set; }
}

internal sealed class ExportarVentasExcelQueryHandler : IRequestHandler<ExportarVentasExcelQuery, ArchivoExcelDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExcelReportGenerator _excel;

    public ExportarVentasExcelQueryHandler(IUnitOfWork unitOfWork, IExcelReportGenerator excel)
    {
        _unitOfWork = unitOfWork;
        _excel = excel;
    }

    public async Task<ArchivoExcelDto?> Handle(ExportarVentasExcelQuery request, CancellationToken cancellationToken)
    {
        var filtro = new FiltroVentasDto
        {
            ClienteId = request.ClienteId,
            Desde     = request.Desde,
            Hasta     = request.Hasta
        };

        var pedidos = (await _unitOfWork.Orders.ObtenerPedidosAsync(filtro)).ToList();
        if (pedidos.Count == 0)
            return null;

        var resumen = await _unitOfWork.Orders.ObtenerResumenPorClienteAsync(filtro);
        var filtros = await DescribirFiltros(filtro);

        return new ArchivoExcelDto
        {
            Contenido     = _excel.GenerarReporteVentas(pedidos, resumen, filtros),
            NombreArchivo = $"ReporteVentas_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
        };
    }

    private async Task<string> DescribirFiltros(FiltroVentasDto filtro)
    {
        var cliente = "Todos";
        if (filtro.ClienteId.HasValue)
        {
            var encontrado = await _unitOfWork.Repository<Client>().GetByIdAsync(filtro.ClienteId.Value);
            cliente = encontrado?.Name ?? $"Id {filtro.ClienteId}";
        }

        var desde = filtro.Desde?.ToString("dd/MM/yyyy") ?? "-";
        var hasta = filtro.Hasta?.ToString("dd/MM/yyyy") ?? "-";

        return $"Cliente: {cliente}  |  Desde: {desde}  |  Hasta: {hasta}";
    }
}
