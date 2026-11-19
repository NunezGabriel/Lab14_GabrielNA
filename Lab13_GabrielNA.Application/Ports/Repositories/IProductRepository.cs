using Lab13_GabrielNA.Application.Dtos;
using Lab13_GabrielNA.Domain.Entities;

namespace Lab13_GabrielNA.Application.Ports.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<ProductoVendidoDto>> ObtenerProductosVendidosAsync(bool soloVendidos);
    Task<IEnumerable<DetalleVentaDto>> ObtenerDetalleVentasAsync(int? productoId);
}
