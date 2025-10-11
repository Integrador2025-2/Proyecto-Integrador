using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.TalentoHumano;
using Backend.Commands.TalentoHumano;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TalentoHumanoController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ITalentoHumanoRepository _repo;
    private readonly IMapper _mapper;

    public TalentoHumanoController(IMediator mediator, ITalentoHumanoRepository repo, IMapper mapper)
    {
        _mediator = mediator;
        _repo = repo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<TalentoHumanoDto>>> GetAll()
    {
        var items = await _mediator.Send(new GetAllTalentoHumanoQuery());
        return Ok(items);
    }

    [HttpGet("rubro/{rubroId}")]
    public async Task<ActionResult<List<TalentoHumanoDto>>> GetByRubro(int rubroId)
    {
        var items = await _repo.GetByRubroIdAsync(rubroId);
        return Ok(items.Select(x => _mapper.Map<TalentoHumanoDto>(x)).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<TalentoHumanoDto>> Create(CreateTalentoHumanoCommand command)
    {
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = created.TalentoHumanoId }, created);
    }
}
