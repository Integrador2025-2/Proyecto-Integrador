using Backend.Commands.CronogramaTareas;
using Backend.Models.DTOs;
using Backend.Queries.CronogramaTareas;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CronogramaTareasController : ControllerBase
{
    private readonly IMediator _mediator;

    public CronogramaTareasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CronogramaTareaDto>>> GetAll()
    {
        var query = new GetAllCronogramaTareasQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CronogramaTareaDto>> GetById(int id)
    {
        var query = new GetCronogramaTareaByIdQuery { CronogramaId = id };
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound();
            
        return Ok(result);
    }

    [HttpGet("tarea/{tareaId}")]
    public async Task<ActionResult<IEnumerable<CronogramaTareaDto>>> GetByTareaId(int tareaId)
    {
        var query = new GetCronogramaTareasByTareaIdQuery { TareaId = tareaId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CronogramaTareaDto>> Create([FromBody] CreateCronogramaTareaCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.CronogramaId }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CronogramaTareaDto>> Update(int id, [FromBody] UpdateCronogramaTareaCommand command)
    {
        if (id != command.CronogramaId)
            return BadRequest();

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteCronogramaTareaCommand { CronogramaId = id };
        var result = await _mediator.Send(command);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}
