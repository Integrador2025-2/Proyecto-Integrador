using Backend.Commands.Entidades;
using Backend.Models.DTOs;
using Backend.Queries.Entidades;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EntidadesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<EntidadesController> _logger;

    public EntidadesController(IMediator mediator, ILogger<EntidadesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las entidades
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EntidadDto>>> GetAll()
    {
        try
        {
            var query = new GetAllEntidadesQuery();
            var entidades = await _mediator.Send(query);
            return Ok(entidades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las entidades");
            return StatusCode(500, new { error = "Error al obtener las entidades", details = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene una entidad por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<EntidadDto>> GetById(int id)
    {
        try
        {
            var query = new GetEntidadByIdQuery { EntidadId = id };
            var entidad = await _mediator.Send(query);

            if (entidad == null)
            {
                return NotFound(new { error = $"Entidad con ID {id} no encontrada" });
            }

            return Ok(entidad);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la entidad con ID {EntidadId}", id);
            return StatusCode(500, new { error = "Error al obtener la entidad", details = ex.Message });
        }
    }

    /// <summary>
    /// Crea una nueva entidad
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<EntidadDto>> Create([FromBody] CreateEntidadCommand command)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(command.Nombre))
            {
                return BadRequest(new { error = "El nombre de la entidad es requerido" });
            }

            var entidad = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = entidad.EntidadId }, entidad);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la entidad");
            return StatusCode(500, new { error = "Error al crear la entidad", details = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza una entidad existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<EntidadDto>> Update(int id, [FromBody] UpdateEntidadCommand command)
    {
        try
        {
            if (id != command.EntidadId)
            {
                return BadRequest(new { error = "El ID de la URL no coincide con el ID del comando" });
            }

            if (string.IsNullOrWhiteSpace(command.Nombre))
            {
                return BadRequest(new { error = "El nombre de la entidad es requerido" });
            }

            var entidad = await _mediator.Send(command);

            if (entidad == null)
            {
                return NotFound(new { error = $"Entidad con ID {id} no encontrada" });
            }

            return Ok(entidad);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Entidad con ID {EntidadId} no encontrada", id);
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la entidad con ID {EntidadId}", id);
            return StatusCode(500, new { error = "Error al actualizar la entidad", details = ex.Message });
        }
    }

    /// <summary>
    /// Elimina una entidad
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteEntidadCommand { EntidadId = id };
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound(new { error = $"Entidad con ID {id} no encontrada" });
            }

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Entidad con ID {EntidadId} no encontrada", id);
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la entidad con ID {EntidadId}", id);
            return StatusCode(500, new { error = "Error al eliminar la entidad", details = ex.Message });
        }
    }
}
