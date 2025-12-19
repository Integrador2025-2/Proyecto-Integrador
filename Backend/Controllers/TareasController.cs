using Backend.Commands.Tareas;
using Backend.Models.DTOs;
using Backend.Queries.Tareas;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TareasController : ControllerBase
{
    private readonly IMediator _mediator;

    public TareasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TareaDto>>> GetAll()
    {
        var query = new GetAllTareasQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TareaDto>> GetById(int id)
    {
        var query = new GetTareaByIdQuery { TareaId = id };
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound();
            
        return Ok(result);
    }

    [HttpGet("actividad/{actividadId}")]
    public async Task<ActionResult<IEnumerable<TareaDto>>> GetByActividadId(int actividadId)
    {
        var query = new GetTareasByActividadIdQuery { ActividadId = actividadId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TareaDto>> Create([FromBody] CreateTareaCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.TareaId }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TareaDto>> Update(int id, [FromBody] UpdateTareaCommand command)
    {
        if (id != command.TareaId)
            return BadRequest();

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteTareaCommand { TareaId = id };
        var result = await _mediator.Send(command);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}
