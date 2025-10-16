using Microsoft.AspNetCore.Mvc;
using MediatR;
using Backend.Models.DTOs;
using Backend.Commands.Tareas;
using Backend.Queries.Tareas;
using System.Threading.Tasks;
using System.Collections.Generic;

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
    public async Task<IEnumerable<TareaDto>> GetAll()
    {
        return await _mediator.Send(new GetAllTareasQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TareaDto>> GetById(int id)
    {
        var dto = await _mediator.Send(new GetTareaByIdQuery { TareaId = id });
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    [HttpGet("byActividad/{actividadId}")]
    public async Task<IEnumerable<TareaDto>> GetByActividad(int actividadId)
    {
        return await _mediator.Send(new GetTareasByActividadQuery { ActividadId = actividadId });
    }

    [HttpPost]
    public async Task<ActionResult<TareaDto>> Create(CreateTareaCommand command)
    {
        var dto = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = dto.TareaId }, dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TareaDto>> Update(int id, UpdateTareaCommand command)
    {
        if (id != command.TareaId) return BadRequest();
        var dto = await _mediator.Send(command);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var ok = await _mediator.Send(new DeleteTareaCommand { TareaId = id });
        if (!ok) return NotFound();
        return NoContent();
    }
}
