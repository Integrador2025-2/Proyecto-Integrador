using Backend.Commands.Recursos;
using Backend.Models.DTOs;
using Backend.Queries.Recursos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RecursosController : ControllerBase
{
    private readonly IMediator _mediator;

    public RecursosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecursoDto>>> GetAll()
    {
        var query = new GetAllRecursosQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RecursoDto>> GetById(int id)
    {
        var query = new GetRecursoByIdQuery { RecursoId = id };
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound();
            
        return Ok(result);
    }

    [HttpGet("actividad/{actividadId}")]
    public async Task<ActionResult<IEnumerable<RecursoDto>>> GetByActividadId(int actividadId)
    {
        var query = new GetRecursosByActividadIdQuery { ActividadId = actividadId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<RecursoDto>> Create([FromBody] CreateRecursoCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.RecursoId }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RecursoDto>> Update(int id, [FromBody] UpdateRecursoCommand command)
    {
        if (id != command.RecursoId)
            return BadRequest();

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteRecursoCommand { RecursoId = id };
        var result = await _mediator.Send(command);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}
