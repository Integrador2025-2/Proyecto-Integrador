using Backend.Commands.RecursosEspecificos;
using Backend.Models.DTOs;
using Backend.Queries.RecursosEspecificos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RecursosEspecificosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RecursosEspecificosController> _logger;

    public RecursosEspecificosController(IMediator mediator, ILogger<RecursosEspecificosController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los recursos específicos
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecursoEspecificoDto>>> GetAll()
    {
        try
        {
            var query = new GetAllRecursosEspecificosQuery();
            var recursos = await _mediator.Send(query);
            return Ok(recursos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los recursos específicos");
            return StatusCode(500, new { error = "Error al obtener los recursos específicos", details = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene un recurso específico por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<RecursoEspecificoDto>> GetById(int id)
    {
        try
        {
            var query = new GetRecursoEspecificoByIdQuery { RecursoEspecificoId = id };
            var recurso = await _mediator.Send(query);

            if (recurso == null)
            {
                return NotFound(new { error = $"Recurso específico con ID {id} no encontrado" });
            }

            return Ok(recurso);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el recurso específico con ID {RecursoEspecificoId}", id);
            return StatusCode(500, new { error = "Error al obtener el recurso específico", details = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene todos los recursos específicos de un recurso
    /// </summary>
    [HttpGet("recurso/{recursoId}")]
    public async Task<ActionResult<IEnumerable<RecursoEspecificoDto>>> GetByRecursoId(int recursoId)
    {
        try
        {
            var query = new GetRecursosEspecificosByRecursoIdQuery { RecursoId = recursoId };
            var recursos = await _mediator.Send(query);
            return Ok(recursos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener recursos específicos del recurso {RecursoId}", recursoId);
            return StatusCode(500, new { error = "Error al obtener recursos específicos", details = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene todos los recursos específicos por tipo
    /// </summary>
    [HttpGet("tipo/{tipo}")]
    public async Task<ActionResult<IEnumerable<RecursoEspecificoDto>>> GetByTipo(string tipo)
    {
        try
        {
            var query = new GetRecursosEspecificosByTipoQuery { Tipo = tipo };
            var recursos = await _mediator.Send(query);
            return Ok(recursos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener recursos específicos del tipo {Tipo}", tipo);
            return StatusCode(500, new { error = "Error al obtener recursos específicos por tipo", details = ex.Message });
        }
    }

    /// <summary>
    /// Crea un nuevo recurso específico
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<RecursoEspecificoDto>> Create([FromBody] CreateRecursoEspecificoCommand command)
    {
        try
        {
            if (command.RecursoId <= 0)
            {
                return BadRequest(new { error = "El RecursoId es requerido" });
            }

            if (string.IsNullOrWhiteSpace(command.Tipo))
            {
                return BadRequest(new { error = "El tipo es requerido" });
            }

            var recurso = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = recurso.RecursoEspecificoId }, recurso);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el recurso específico");
            return StatusCode(500, new { error = "Error al crear el recurso específico", details = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza un recurso específico existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<RecursoEspecificoDto>> Update(int id, [FromBody] UpdateRecursoEspecificoCommand command)
    {
        try
        {
            if (id != command.RecursoEspecificoId)
            {
                return BadRequest(new { error = "El ID de la URL no coincide con el ID del comando" });
            }

            if (string.IsNullOrWhiteSpace(command.Tipo))
            {
                return BadRequest(new { error = "El tipo es requerido" });
            }

            var recurso = await _mediator.Send(command);

            if (recurso == null)
            {
                return NotFound(new { error = $"Recurso específico con ID {id} no encontrado" });
            }

            return Ok(recurso);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Recurso específico con ID {RecursoEspecificoId} no encontrado", id);
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el recurso específico con ID {RecursoEspecificoId}", id);
            return StatusCode(500, new { error = "Error al actualizar el recurso específico", details = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un recurso específico
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteRecursoEspecificoCommand { RecursoEspecificoId = id };
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound(new { error = $"Recurso específico con ID {id} no encontrado" });
            }

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Recurso específico con ID {RecursoEspecificoId} no encontrado", id);
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el recurso específico con ID {RecursoEspecificoId}", id);
            return StatusCode(500, new { error = "Error al eliminar el recurso específico", details = ex.Message });
        }
    }
}
