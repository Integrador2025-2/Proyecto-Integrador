using Backend.Commands.Actividades;
using Backend.Models.DTOs;
using Backend.Queries.Actividades;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActividadesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ActividadesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActividadDto>>> GetAll()
    {
        var actividades = await _mediator.Send(new GetAllActividadesQuery());
        return Ok(actividades);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActividadDto>> GetById(int id)
    {
        var actividad = await _mediator.Send(new GetActividadByIdQuery { ActividadId = id });
        if (actividad == null)
            return NotFound();

        return Ok(actividad);
    }

    [HttpGet("cadena-de-valor/{cadenaDeValorId}")]
    public async Task<ActionResult<IEnumerable<ActividadDto>>> GetByCadenaDeValorId(int cadenaDeValorId)
    {
        var actividades = await _mediator.Send(new GetActividadesByCadenaDeValorIdQuery { CadenaDeValorId = cadenaDeValorId });
        return Ok(actividades);
    }

    [HttpPost]
    public async Task<ActionResult<ActividadDto>> Create([FromBody] CreateActividadCommand command)
    {
        var actividad = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = actividad.ActividadId }, actividad);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ActividadDto>> Update(int id, [FromBody] UpdateActividadCommand command)
    {
        if (id != command.ActividadId)
            return BadRequest("ID mismatch");

        try
        {
            var actividad = await _mediator.Send(command);
            return Ok(actividad);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteActividadCommand { ActividadId = id });
        if (!result)
            return NotFound();

        return NoContent();
    }
}
