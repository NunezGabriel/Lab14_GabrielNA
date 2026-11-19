using Lab13_GabrielNA.Application.Dtos;
using Lab13_GabrielNA.Domain.Entities;

namespace Lab13_GabrielNA.Application.Ports.Repositories;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IEnumerable<PedidoReporteDto>> ObtenerPedidosAsync(FiltroVentasDto filtro);
    Task<IEnumerable<ResumenClienteDto>> ObtenerResumenPorClienteAsync(FiltroVentasDto filtro);
}
