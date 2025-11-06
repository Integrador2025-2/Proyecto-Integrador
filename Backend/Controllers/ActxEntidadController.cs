using Backend.Commands.ActxEntidad;
using Backend.Models.DTOs;
using Backend.Queries.ActxEntidad;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActXEntidadController : ControllerBase
{
    private readonly IMediator _mediator;

    public ActXEntidadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActXEntidadDto>>> GetAll()
    {
        var query = new GetAllActXEntidadesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActXEntidadDto>> GetById(int id)
    {
        var query = new GetActXEntidadByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound();
            
        return Ok(result);
    }

    [HttpGet("actividad/{actividadId}")]
    public async Task<ActionResult<IEnumerable<ActXEntidadDto>>> GetByActividadId(int actividadId)
    {
        var query = new GetActXEntidadesByActividadIdQuery { ActividadId = actividadId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("entidad/{entidadId}")]
    public async Task<ActionResult<IEnumerable<ActXEntidadDto>>> GetByEntidadId(int entidadId)
    {
        var query = new GetActXEntidadesByEntidadIdQuery { EntidadId = entidadId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ActXEntidadDto>> Create([FromBody] CreateActXEntidadCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ActXEntidadDto>> Update(int id, [FromBody] UpdateActXEntidadCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteActXEntidadCommand { Id = id };
        var result = await _mediator.Send(command);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}
