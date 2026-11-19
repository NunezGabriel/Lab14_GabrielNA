using Lab13_GabrielNA.Application.Dtos;
using Lab13_GabrielNA.Application.Ports.Repositories;
using MediatR;

namespace Lab13_GabrielNA.Application.Features.Reportes.Queries;

public record GetProductosVendidosQuery : IRequest<IEnumerable<ProductoVendidoDto>>
{
    public bool SoloVendidos { get; set; }
}

internal sealed class GetProductosVendidosQueryHandler : IRequestHandler<GetProductosVendidosQuery, IEnumerable<ProductoVendidoDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductosVendidosQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ProductoVendidoDto>> Handle(GetProductosVendidosQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Products.ObtenerProductosVendidosAsync(request.SoloVendidos);
    }
}
