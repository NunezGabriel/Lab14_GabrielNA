using Lab13_GabrielNA.Application.Features.Clientes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lab13_GabrielNA.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetClientesQuery()));
    }
}
