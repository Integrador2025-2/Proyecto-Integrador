using Backend.Commands.Proyectos;
using Backend.Models.DTOs;
using Backend.Queries.Proyectos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProyectosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProyectosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProyectoDto>>> GetAll()
    {
        var proyectos = await _mediator.Send(new GetAllProyectosQuery());
        return Ok(proyectos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProyectoDto>> GetById(int id)
    {
        var proyecto = await _mediator.Send(new GetProyectoByIdQuery { ProyectoId = id });
        if (proyecto == null)
            return NotFound();

        return Ok(proyecto);
    }

    [HttpGet("usuario/{usuarioId}")]
    public async Task<ActionResult<IEnumerable<ProyectoDto>>> GetByUsuarioId(int usuarioId)
    {
        var proyectos = await _mediator.Send(new GetProyectosByUsuarioIdQuery { UsuarioId = usuarioId });
        return Ok(proyectos);
    }

    [HttpPost]
    public async Task<ActionResult<ProyectoDto>> Create([FromBody] CreateProyectoCommand command)
    {
        var proyecto = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = proyecto.ProyectoId }, proyecto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProyectoDto>> Update(int id, [FromBody] UpdateProyectoCommand command)
    {
        if (id != command.ProyectoId)
            return BadRequest("ID mismatch");

        try
        {
            var proyecto = await _mediator.Send(command);
            return Ok(proyecto);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteProyectoCommand { ProyectoId = id });
        if (!result)
            return NotFound();

        return NoContent();
    }
}
