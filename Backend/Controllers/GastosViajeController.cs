using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.GastosViaje;
using Backend.Commands.GastosViaje;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GastosViajeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IGastosViajeRepository _repo;
    private readonly IMapper _mapper;

    public GastosViajeController(IMediator mediator, IGastosViajeRepository repo, IMapper mapper)
    {
        _mediator = mediator;
        _repo = repo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<GastosViajeDto>>> GetAll()
    {
        var items = await _mediator.Send(new GetAllGastosViajeQuery());
        return Ok(items);
    }

    [HttpGet("rubro/{rubroId}")]
    public async Task<ActionResult<List<GastosViajeDto>>> GetByRubro(int rubroId)
    {
        var items = await _repo.GetByRubroIdAsync(rubroId);
        return Ok(items.Select(x => _mapper.Map<GastosViajeDto>(x)).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<GastosViajeDto>> Create(CreateGastosViajeCommand command)
    {
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = created.GastosViajeId }, created);
    }
}
