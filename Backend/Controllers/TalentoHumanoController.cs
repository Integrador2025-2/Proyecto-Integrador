using Backend.Commands.TalentoHumano;
using Backend.Queries.TalentoHumano;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/talentohumano")]
[Authorize]
public class TalentoHumanoController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TalentoHumanoController> _logger;

    public TalentoHumanoController(IMediator mediator, ILogger<TalentoHumanoController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllTalentoHumanoQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetTalentoHumanoByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (result == null)
        {
            return NotFound($"TalentoHumano with ID {id} not found.");
        }
        
        return Ok(result);
    }

    [HttpGet("recurso-especifico/{recursoEspecificoId}")]
    public async Task<IActionResult> GetByRecursoEspecificoId(int recursoEspecificoId)
    {
        var query = new GetTalentoHumanoByRecursoEspecificoIdQuery(recursoEspecificoId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTalentoHumanoCommand command)
    {
        if (command.RecursoEspecificoId <= 0)
        {
            return BadRequest("RecursoEspecificoId must be greater than 0.");
        }

        if (command.ContratacionId <= 0)
        {
            return BadRequest("ContratacionId must be greater than 0.");
        }

        if (string.IsNullOrWhiteSpace(command.CargoEspecifico))
        {
            return BadRequest("CargoEspecifico is required.");
        }

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.TalentoHumanoId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTalentoHumanoCommand command)
    {
        if (id != command.TalentoHumanoId)
        {
            return BadRequest("ID mismatch.");
        }

        if (command.ContratacionId <= 0)
        {
            return BadRequest("ContratacionId must be greater than 0.");
        }

        if (string.IsNullOrWhiteSpace(command.CargoEspecifico))
        {
            return BadRequest("CargoEspecifico is required.");
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
        var command = new DeleteTalentoHumanoCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}
