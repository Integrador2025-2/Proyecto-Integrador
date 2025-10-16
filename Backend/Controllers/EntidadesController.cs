using Backend.Commands.Entidades;
using Backend.Models.DTOs;
using Backend.Queries.Entidades;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EntidadesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EntidadesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<EntidadDto>>> GetAll([FromQuery] string? searchTerm = null)
    {
        var query = new GetAllEntidadesQuery { SearchTerm = searchTerm };
        var items = await _mediator.Send(query);
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EntidadDto>> GetById(int id)
    {
        var query = new GetEntidadByIdQuery { Id = id };
        var item = await _mediator.Send(query);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<EntidadDto>> Create([FromBody] CreateEntidadCommand command)
    {
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = created.EntidadId }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EntidadDto>> Update(int id, [FromBody] UpdateEntidadCommand command)
    {
        if (id != command.EntidadId) return BadRequest("ID mismatch");
        var updated = await _mediator.Send(command);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteEntidadCommand { EntidadId = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
