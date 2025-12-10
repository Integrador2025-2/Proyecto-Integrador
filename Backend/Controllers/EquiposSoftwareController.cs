using Backend.Commands.EquiposSoftware;
using Backend.Queries.EquiposSoftware;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquiposSoftwareController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<EquiposSoftwareController> _logger;

    public EquiposSoftwareController(IMediator mediator, ILogger<EquiposSoftwareController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllEquiposSoftwareQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all equipos software");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var query = new GetEquiposSoftwareByIdQuery { EquiposSoftwareId = id };
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound($"EquiposSoftware with ID {id} not found");
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting equipos software by ID: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("recurso-especifico/{recursoEspecificoId}")]
    public async Task<IActionResult> GetByRecursoEspecificoId(int recursoEspecificoId)
    {
        try
        {
            var query = new GetEquiposSoftwareByRecursoEspecificoIdQuery { RecursoEspecificoId = recursoEspecificoId };
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound($"EquiposSoftware with RecursoEspecificoId {recursoEspecificoId} not found");
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting equipos software by RecursoEspecificoId: {RecursoEspecificoId}", recursoEspecificoId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEquiposSoftwareCommand command)
    {
        try
        {
            if (command.RecursoEspecificoId <= 0)
            {
                return BadRequest("RecursoEspecificoId must be greater than 0");
            }

            if (string.IsNullOrWhiteSpace(command.EspecificacionesTecnicas))
            {
                return BadRequest("EspecificacionesTecnicas is required");
            }

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.EquiposSoftwareId }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating equipos software");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEquiposSoftwareCommand command)
    {
        try
        {
            if (id != command.EquiposSoftwareId)
            {
                return BadRequest("ID mismatch");
            }

            if (string.IsNullOrWhiteSpace(command.EspecificacionesTecnicas))
            {
                return BadRequest("EspecificacionesTecnicas is required");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "EquiposSoftware not found: {Id}", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating equipos software: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteEquiposSoftwareCommand { EquiposSoftwareId = id };
            var result = await _mediator.Send(command);
            
            if (!result)
            {
                return NotFound($"EquiposSoftware with ID {id} not found");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting equipos software: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}
