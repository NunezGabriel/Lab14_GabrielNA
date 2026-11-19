using Lab13_GabrielNA.Application.Dtos;
using Lab13_GabrielNA.Application.Ports.Repositories;
using Lab13_GabrielNA.Domain.Entities;
using MediatR;

namespace Lab13_GabrielNA.Application.Features.Clientes.Queries;

public record GetClientesQuery : IRequest<IEnumerable<ClienteDto>>;

internal sealed class GetClientesQueryHandler : IRequestHandler<GetClientesQuery, IEnumerable<ClienteDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetClientesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ClienteDto>> Handle(GetClientesQuery request, CancellationToken cancellationToken)
    {
        var clientes = await _unitOfWork.Repository<Client>().GetAllAsync();

        return clientes
            .OrderBy(c => c.ClientId)
            .Select(c => new ClienteDto
            {
                ClienteId = c.ClientId,
                Nombre    = c.Name,
                Email     = c.Email
            });
    }
}
