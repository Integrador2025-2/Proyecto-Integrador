using Backend.Commands.ProteccionConocimientoDivulgacion;
using Backend.Queries.ProteccionConocimientoDivulgacion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProteccionConocimientoDivulgacionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProteccionConocimientoDivulgacionController> _logger;

    public ProteccionConocimientoDivulgacionController(IMediator mediator, ILogger<ProteccionConocimientoDivulgacionController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllProteccionConocimientoDivulgacionQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all proteccion conocimiento divulgacion");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var query = new GetProteccionConocimientoDivulgacionByIdQuery { ProteccionId = id };
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound($"ProteccionConocimientoDivulgacion with ID {id} not found");
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting proteccion conocimiento divulgacion by ID: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("recurso-especifico/{recursoEspecificoId}")]
    public async Task<IActionResult> GetByRecursoEspecificoId(int recursoEspecificoId)
    {
        try
        {
            var query = new GetProteccionConocimientoDivulgacionByRecursoEspecificoIdQuery { RecursoEspecificoId = recursoEspecificoId };
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound($"ProteccionConocimientoDivulgacion with RecursoEspecificoId {recursoEspecificoId} not found");
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting proteccion conocimiento divulgacion by RecursoEspecificoId: {RecursoEspecificoId}", recursoEspecificoId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProteccionConocimientoDivulgacionCommand command)
    {
        try
        {
            if (command.RecursoEspecificoId <= 0)
            {
                return BadRequest("RecursoEspecificoId must be greater than 0");
            }

            if (string.IsNullOrWhiteSpace(command.ActividadHapat))
            {
                return BadRequest("ActividadHapat is required");
            }

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.ProteccionId }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating proteccion conocimiento divulgacion");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProteccionConocimientoDivulgacionCommand command)
    {
        try
        {
            if (id != command.ProteccionId)
            {
                return BadRequest("ID mismatch");
            }

            if (string.IsNullOrWhiteSpace(command.ActividadHapat))
            {
                return BadRequest("ActividadHapat is required");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "ProteccionConocimientoDivulgacion not found: {Id}", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating proteccion conocimiento divulgacion: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteProteccionConocimientoDivulgacionCommand { ProteccionId = id };
            var result = await _mediator.Send(command);
            
            if (!result)
            {
                return NotFound($"ProteccionConocimientoDivulgacion with ID {id} not found");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting proteccion conocimiento divulgacion: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}
