using Lab13_GabrielNA.Application.Dtos;

namespace Lab13_GabrielNA.Application.Ports.Reports;

public interface IExcelReportGenerator
{
    byte[] GenerarReporteVentas(IEnumerable<PedidoReporteDto> pedidos, IEnumerable<ResumenClienteDto> resumen, string filtrosAplicados);
    byte[] GenerarReporteProductos(IEnumerable<ProductoVendidoDto> productos, IEnumerable<DetalleVentaDto> detalle, string filtrosAplicados);
}
