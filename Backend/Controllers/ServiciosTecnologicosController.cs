using MediatR;
using Microsoft.AspNetCore.Mvc;
using Backend.Models.DTOs;
using Backend.Queries.ServiciosTecnologicos;
using Backend.Commands.ServiciosTecnologicos;
using Backend.Infrastructure.Repositories;
using AutoMapper;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiciosTecnologicosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IServiciosTecnologicosRepository _repo;
    private readonly IMapper _mapper;

    public ServiciosTecnologicosController(IMediator mediator, IServiciosTecnologicosRepository repo, IMapper mapper)
    {
        _mediator = mediator;
        _repo = repo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<ServiciosTecnologicosDto>>> GetAll()
    {
        var items = await _repo.GetAllAsync();
        return Ok(items.Select(x => _mapper.Map<ServiciosTecnologicosDto>(x)).ToList());
    }

    [HttpGet("rubro/{rubroId}")]
    public async Task<ActionResult<List<ServiciosTecnologicosDto>>> GetByRubro(int rubroId)
    {
        var items = await _repo.GetByRubroIdAsync(rubroId);
        return Ok(items.Select(x => _mapper.Map<ServiciosTecnologicosDto>(x)).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiciosTecnologicosDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetServiciosTecnologicosByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ServiciosTecnologicosDto>> Create([FromBody] CreateServiciosTecnologicosCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.ServiciosTecnologicosId }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ServiciosTecnologicosDto>> Update(int id, [FromBody] UpdateServiciosTecnologicosCommand command)
    {
        if (id != command.ServiciosTecnologicosId) return BadRequest();
        var result = await _mediator.Send(command);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteServiciosTecnologicosCommand(id));
        if (!deleted) return NotFound();
        return NoContent();
    }
}
