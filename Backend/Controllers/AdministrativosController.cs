using Backend.Commands.Administrativos;
using Backend.Queries.Administrativos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdministrativosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AdministrativosController> _logger;

    public AdministrativosController(IMediator mediator, ILogger<AdministrativosController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllAdministrativosQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all administrativos");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var query = new GetAdministrativosByIdQuery { AdministrativoId = id };
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound($"Administrativos with ID {id} not found");
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting administrativos by ID: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("recurso-especifico/{recursoEspecificoId}")]
    public async Task<IActionResult> GetByRecursoEspecificoId(int recursoEspecificoId)
    {
        try
        {
            var query = new GetAdministrativosByRecursoEspecificoIdQuery { RecursoEspecificoId = recursoEspecificoId };
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound($"Administrativos with RecursoEspecificoId {recursoEspecificoId} not found");
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting administrativos by RecursoEspecificoId: {RecursoEspecificoId}", recursoEspecificoId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAdministrativosCommand command)
    {
        try
        {
            if (command.RecursoEspecificoId <= 0)
            {
                return BadRequest("RecursoEspecificoId must be greater than 0");
            }

            if (string.IsNullOrWhiteSpace(command.Cargo))
            {
                return BadRequest("Cargo is required");
            }

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.AdministrativoId }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating administrativos");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAdministrativosCommand command)
    {
        try
        {
            if (id != command.AdministrativoId)
            {
                return BadRequest("ID mismatch");
            }

            if (string.IsNullOrWhiteSpace(command.Cargo))
            {
                return BadRequest("Cargo is required");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Administrativos not found: {Id}", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating administrativos: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteAdministrativosCommand { AdministrativoId = id };
            var result = await _mediator.Send(command);
            
            if (!result)
            {
                return NotFound($"Administrativos with ID {id} not found");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting administrativos: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}
