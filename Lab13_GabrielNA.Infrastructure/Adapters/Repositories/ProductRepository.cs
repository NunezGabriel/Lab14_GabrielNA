using Lab13_GabrielNA.Application.Dtos;
using Lab13_GabrielNA.Application.Ports.Repositories;
using Lab13_GabrielNA.Domain.Entities;
using Lab13_GabrielNA.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Lab13_GabrielNA.Infrastructure.Adapters.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProductoVendidoDto>> ObtenerProductosVendidosAsync(bool soloVendidos)
    {
        var query = _dbSet.AsNoTracking();

        if (soloVendidos)
            query = query.Where(p => p.OrderDetails.Any());

        return await query
            .Select(p => new ProductoVendidoDto
            {
                ProductoId       = p.ProductId,
                Producto         = p.Name,
                Descripcion      = p.Description,
                Precio           = p.Price,
                UnidadesVendidas = p.OrderDetails.Sum(d => d.Quantity),
                Ingresos         = Math.Round(p.OrderDetails.Sum(d => d.Quantity * p.Price), 2)
            })
            .OrderByDescending(p => p.Ingresos)
            .ThenBy(p => p.Producto)
            .ToListAsync();
    }

    public async Task<IEnumerable<DetalleVentaDto>> ObtenerDetalleVentasAsync(int? productoId)
    {
        var query = _context.OrderDetails.AsNoTracking();

        if (productoId.HasValue)
        {
            var id = productoId.Value;
            query = query.Where(d => d.ProductId == id);
        }

        return await query
            .OrderBy(d => d.Order.OrderDate)
            .ThenBy(d => d.OrderId)
            .ThenBy(d => d.Product.Name)
            .Select(d => new DetalleVentaDto
            {
                PedidoId       = d.OrderId,
                Fecha          = d.Order.OrderDate,
                Cliente        = d.Order.Client.Name,
                Producto       = d.Product.Name,
                Cantidad       = d.Quantity,
                PrecioUnitario = d.Product.Price,
                Subtotal       = Math.Round(d.Quantity * d.Product.Price, 2)
            })
            .ToListAsync();
    }
}
