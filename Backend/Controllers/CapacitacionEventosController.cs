using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.CapacitacionEventos;
using Backend.Commands.CapacitacionEventos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CapacitacionEventosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICapacitacionEventosRepository _repo;
    private readonly IMapper _mapper;

    public CapacitacionEventosController(IMediator mediator, ICapacitacionEventosRepository repo, IMapper mapper)
    {
        _mediator = mediator;
        _repo = repo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<CapacitacionEventosDto>>> GetAll()
    {
        var items = await _mediator.Send(new GetAllCapacitacionEventosQuery());
        return Ok(items);
    }

    [HttpGet("rubro/{rubroId}")]
    public async Task<ActionResult<List<CapacitacionEventosDto>>> GetByRubro(int rubroId)
    {
        var items = await _repo.GetByRubroIdAsync(rubroId);
        return Ok(items.Select(x => _mapper.Map<CapacitacionEventosDto>(x)).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<CapacitacionEventosDto>> Create(CreateCapacitacionEventosCommand command)
    {
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = created.CapacitacionEventosId }, created);
    }
}
