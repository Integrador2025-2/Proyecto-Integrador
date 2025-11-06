using Backend.Commands.Objetivos;
using Backend.Models.DTOs;
using Backend.Queries.Objetivos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ObjetivosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ObjetivosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ObjetivoDto>>> GetAll()
    {
        var query = new GetAllObjetivosQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ObjetivoDto>> GetById(int id)
    {
        var query = new GetObjetivoByIdQuery { ObjetivoId = id };
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound();
            
        return Ok(result);
    }

    [HttpGet("proyecto/{proyectoId}")]
    public async Task<ActionResult<IEnumerable<ObjetivoDto>>> GetByProyectoId(int proyectoId)
    {
        var query = new GetObjetivosByProyectoIdQuery { ProyectoId = proyectoId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ObjetivoDto>> Create([FromBody] CreateObjetivoCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.ObjetivoId }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ObjetivoDto>> Update(int id, [FromBody] UpdateObjetivoCommand command)
    {
        if (id != command.ObjetivoId)
            return BadRequest();

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteObjetivoCommand { ObjetivoId = id };
        var result = await _mediator.Send(command);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}
