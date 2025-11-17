using Backend.Commands.TalentoHumanoTareas;
using Backend.Queries.TalentoHumanoTareas;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/talentohumanotareas")]
[Authorize]
public class TalentoHumanoTareasController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TalentoHumanoTareasController> _logger;

    public TalentoHumanoTareasController(IMediator mediator, ILogger<TalentoHumanoTareasController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllTalentoHumanoTareasQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetTalentoHumanoTareaByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (result == null)
        {
            return NotFound($"TalentoHumanoTarea with ID {id} not found.");
        }
        
        return Ok(result);
    }

    [HttpGet("talentohumano/{talentoHumanoId}")]
    public async Task<IActionResult> GetByTalentoHumanoId(int talentoHumanoId)
    {
        var query = new GetTalentoHumanoTareasByTalentoHumanoIdQuery(talentoHumanoId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("tarea/{tareaId}")]
    public async Task<IActionResult> GetByTareaId(int tareaId)
    {
        var query = new GetTalentoHumanoTareasByTareaIdQuery(tareaId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTalentoHumanoTareaCommand command)
    {
        if (command.TalentoHumanoId <= 0)
        {
            return BadRequest("TalentoHumanoId must be greater than 0.");
        }

        if (command.Tarea <= 0)
        {
            return BadRequest("Tarea must be greater than 0.");
        }

        if (string.IsNullOrWhiteSpace(command.RolenTarea))
        {
            return BadRequest("RolenTarea is required.");
        }

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.TalentoHumanoTareasId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTalentoHumanoTareaCommand command)
    {
        if (id != command.TalentoHumanoTareasId)
        {
            return BadRequest("ID mismatch.");
        }

        if (command.TalentoHumanoId <= 0)
        {
            return BadRequest("TalentoHumanoId must be greater than 0.");
        }

        if (command.Tarea <= 0)
        {
            return BadRequest("Tarea must be greater than 0.");
        }

        if (string.IsNullOrWhiteSpace(command.RolenTarea))
        {
            return BadRequest("RolenTarea is required.");
        }

        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteTalentoHumanoTareaCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}
