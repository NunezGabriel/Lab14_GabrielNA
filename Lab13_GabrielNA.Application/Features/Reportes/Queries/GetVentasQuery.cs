using Lab13_GabrielNA.Application.Dtos;
using Lab13_GabrielNA.Application.Ports.Repositories;
using MediatR;

namespace Lab13_GabrielNA.Application.Features.Reportes.Queries;

public record GetVentasQuery : IRequest<IEnumerable<PedidoReporteDto>>
{
    public int? ClienteId { get; set; }
    public DateTime? Desde { get; set; }
    public DateTime? Hasta { get; set; }
}

internal sealed class GetVentasQueryHandler : IRequestHandler<GetVentasQuery, IEnumerable<PedidoReporteDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetVentasQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PedidoReporteDto>> Handle(GetVentasQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Orders.ObtenerPedidosAsync(new FiltroVentasDto
        {
            ClienteId = request.ClienteId,
            Desde     = request.Desde,
            Hasta     = request.Hasta
        });
    }
}
