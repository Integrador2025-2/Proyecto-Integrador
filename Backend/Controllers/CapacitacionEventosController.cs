using Backend.Commands.CapacitacionEventos;
using Backend.Queries.CapacitacionEventos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CapacitacionEventosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CapacitacionEventosController> _logger;

    public CapacitacionEventosController(IMediator mediator, ILogger<CapacitacionEventosController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllCapacitacionEventosQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all capacitacion eventos");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var query = new GetCapacitacionEventosByIdQuery { CapacitacionEventosId = id };
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound($"CapacitacionEventos with ID {id} not found");
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting capacitacion eventos by ID: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("recurso-especifico/{recursoEspecificoId}")]
    public async Task<IActionResult> GetByRecursoEspecificoId(int recursoEspecificoId)
    {
        try
        {
            var query = new GetCapacitacionEventosByRecursoEspecificoIdQuery { RecursoEspecificoId = recursoEspecificoId };
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound($"CapacitacionEventos with RecursoEspecificoId {recursoEspecificoId} not found");
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting capacitacion eventos by RecursoEspecificoId: {RecursoEspecificoId}", recursoEspecificoId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCapacitacionEventosCommand command)
    {
        try
        {
            if (command.RecursoEspecificoId <= 0)
            {
                return BadRequest("RecursoEspecificoId must be greater than 0");
            }

            if (string.IsNullOrWhiteSpace(command.Tema))
            {
                return BadRequest("Tema is required");
            }

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.CapacitacionEventosId }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating capacitacion eventos");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCapacitacionEventosCommand command)
    {
        try
        {
            if (id != command.CapacitacionEventosId)
            {
                return BadRequest("ID mismatch");
            }

            if (string.IsNullOrWhiteSpace(command.Tema))
            {
                return BadRequest("Tema is required");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "CapacitacionEventos not found: {Id}", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating capacitacion eventos: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteCapacitacionEventosCommand { CapacitacionEventosId = id };
            var result = await _mediator.Send(command);
            
            if (!result)
            {
                return NotFound($"CapacitacionEventos with ID {id} not found");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting capacitacion eventos: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}
