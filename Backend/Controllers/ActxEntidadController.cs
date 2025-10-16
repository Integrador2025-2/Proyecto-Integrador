using Backend.Commands.ActxEntidad;
using Backend.Models.DTOs;
using Backend.Queries.ActxEntidad;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActxEntidadController : ControllerBase
{
    private readonly IMediator _mediator;

    public ActxEntidadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ActxEntidadDto>>> GetAll([FromQuery] int? actividadId = null)
    {
        var query = new GetAllActxEntidadQuery { ActividadId = actividadId };
        var items = await _mediator.Send(query);
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActxEntidadDto>> GetById(int id)
    {
        var query = new GetActxEntidadByIdQuery { Id = id };
        var item = await _mediator.Send(query);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ActxEntidadDto>> Create([FromBody] CreateActxEntidadCommand command)
    {
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = created.ActXEntidadId }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ActxEntidadDto>> Update(int id, [FromBody] UpdateActxEntidadCommand command)
    {
        if (id != command.ActXEntidadId) return BadRequest("ID mismatch");
        var updated = await _mediator.Send(command);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteActxEntidadCommand { ActXEntidadId = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
