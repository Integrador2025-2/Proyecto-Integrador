using Backend.Commands.SeguimientoEvaluacion;
using Backend.Queries.SeguimientoEvaluacion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SeguimientoEvaluacionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SeguimientoEvaluacionController> _logger;

    public SeguimientoEvaluacionController(IMediator mediator, ILogger<SeguimientoEvaluacionController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllSeguimientoEvaluacionQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all seguimiento evaluacion");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var query = new GetSeguimientoEvaluacionByIdQuery { SeguimientoId = id };
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound($"SeguimientoEvaluacion with ID {id} not found");
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting seguimiento evaluacion by ID: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("recurso-especifico/{recursoEspecificoId}")]
    public async Task<IActionResult> GetByRecursoEspecificoId(int recursoEspecificoId)
    {
        try
        {
            var query = new GetSeguimientoEvaluacionByRecursoEspecificoIdQuery { RecursoEspecificoId = recursoEspecificoId };
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound($"SeguimientoEvaluacion with RecursoEspecificoId {recursoEspecificoId} not found");
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting seguimiento evaluacion by RecursoEspecificoId: {RecursoEspecificoId}", recursoEspecificoId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSeguimientoEvaluacionCommand command)
    {
        try
        {
            if (command.RecursoEspecificoId <= 0)
            {
                return BadRequest("RecursoEspecificoId must be greater than 0");
            }

            if (string.IsNullOrWhiteSpace(command.CargoResponsable))
            {
                return BadRequest("CargoResponsable is required");
            }

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.SeguimientoId }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating seguimiento evaluacion");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSeguimientoEvaluacionCommand command)
    {
        try
        {
            if (id != command.SeguimientoId)
            {
                return BadRequest("ID mismatch");
            }

            if (string.IsNullOrWhiteSpace(command.CargoResponsable))
            {
                return BadRequest("CargoResponsable is required");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "SeguimientoEvaluacion not found: {Id}", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating seguimiento evaluacion: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteSeguimientoEvaluacionCommand { SeguimientoId = id };
            var result = await _mediator.Send(command);
            
            if (!result)
            {
                return NotFound($"SeguimientoEvaluacion with ID {id} not found");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting seguimiento evaluacion: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}
