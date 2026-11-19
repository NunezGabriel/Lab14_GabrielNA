using Lab13_GabrielNA.Application.Dtos;
using Lab13_GabrielNA.Application.Ports.Repositories;
using Lab13_GabrielNA.Domain.Entities;
using Lab13_GabrielNA.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Lab13_GabrielNA.Infrastructure.Adapters.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PedidoReporteDto>> ObtenerPedidosAsync(FiltroVentasDto filtro)
    {
        return await Filtrar(filtro)
            .OrderBy(o => o.OrderDate)
            .ThenBy(o => o.OrderId)
            .Select(o => new PedidoReporteDto
            {
                PedidoId  = o.OrderId,
                Fecha     = o.OrderDate,
                ClienteId = o.ClientId,
                Cliente   = o.Client.Name,
                Email     = o.Client.Email,
                Productos = o.OrderDetails.Count,
                Unidades  = o.OrderDetails.Sum(d => d.Quantity),
                Total     = Math.Round(o.OrderDetails.Sum(d => d.Quantity * d.Product.Price), 2)
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<ResumenClienteDto>> ObtenerResumenPorClienteAsync(FiltroVentasDto filtro)
    {
        var pedidos = await ObtenerPedidosAsync(filtro);

        return pedidos
            .GroupBy(p => new { p.ClienteId, p.Cliente })
            .Select(g => new ResumenClienteDto
            {
                ClienteId = g.Key.ClienteId,
                Cliente   = g.Key.Cliente,
                Pedidos   = g.Count(),
                Unidades  = g.Sum(p => p.Unidades),
                Total     = g.Sum(p => p.Total)
            })
            .OrderByDescending(r => r.Total)
            .ThenBy(r => r.Cliente)
            .ToList();
    }

    private IQueryable<Order> Filtrar(FiltroVentasDto filtro)
    {
        var query = _dbSet.AsNoTracking();

        if (filtro.ClienteId.HasValue)
        {
            var clienteId = filtro.ClienteId.Value;
            query = query.Where(o => o.ClientId == clienteId);
        }

        if (filtro.Desde.HasValue)
        {
            var desde = filtro.Desde.Value.Date;
            query = query.Where(o => o.OrderDate >= desde);
        }

        if (filtro.Hasta.HasValue)
        {
            var hasta = filtro.Hasta.Value.Date.AddDays(1);
            query = query.Where(o => o.OrderDate < hasta);
        }

        return query;
    }
}
