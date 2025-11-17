using Backend.Commands.ServiciosTecnologicos;
using Backend.Queries.ServiciosTecnologicos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServiciosTecnologicosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ServiciosTecnologicosController> _logger;

    public ServiciosTecnologicosController(IMediator mediator, ILogger<ServiciosTecnologicosController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllServiciosTecnologicosQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all servicios tecnologicos");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var query = new GetServiciosTecnologicosByIdQuery { ServiciosTecnologicosId = id };
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound($"ServiciosTecnologicos with ID {id} not found");
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting servicios tecnologicos by ID: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("recurso-especifico/{recursoEspecificoId}")]
    public async Task<IActionResult> GetByRecursoEspecificoId(int recursoEspecificoId)
    {
        try
        {
            var query = new GetServiciosTecnologicosByRecursoEspecificoIdQuery { RecursoEspecificoId = recursoEspecificoId };
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound($"ServiciosTecnologicos with RecursoEspecificoId {recursoEspecificoId} not found");
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting servicios tecnologicos by RecursoEspecificoId: {RecursoEspecificoId}", recursoEspecificoId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateServiciosTecnologicosCommand command)
    {
        try
        {
            if (command.RecursoEspecificoId <= 0)
            {
                return BadRequest("RecursoEspecificoId must be greater than 0");
            }

            if (string.IsNullOrWhiteSpace(command.Descripcion))
            {
                return BadRequest("Descripcion is required");
            }

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.ServiciosTecnologicosId }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating servicios tecnologicos");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateServiciosTecnologicosCommand command)
    {
        try
        {
            if (id != command.ServiciosTecnologicosId)
            {
                return BadRequest("ID mismatch");
            }

            if (string.IsNullOrWhiteSpace(command.Descripcion))
            {
                return BadRequest("Descripcion is required");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "ServiciosTecnologicos not found: {Id}", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating servicios tecnologicos: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteServiciosTecnologicosCommand { ServiciosTecnologicosId = id };
            var result = await _mediator.Send(command);
            
            if (!result)
            {
                return NotFound($"ServiciosTecnologicos with ID {id} not found");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting servicios tecnologicos: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}
