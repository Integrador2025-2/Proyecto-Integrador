using Backend.Commands.Rubros;
using Backend.Models.DTOs;
using Backend.Queries.Rubros;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RubrosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RubrosController> _logger;

    public RubrosController(IMediator mediator, ILogger<RubrosController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los rubros
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RubroDto>>> GetAll()
    {
        try
        {
            var query = new GetAllRubrosQuery();
            var rubros = await _mediator.Send(query);
            return Ok(rubros);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los rubros");
            return StatusCode(500, new { error = "Error al obtener los rubros", details = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene un rubro por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<RubroDto>> GetById(int id)
    {
        try
        {
            var query = new GetRubroByIdQuery { RubroId = id };
            var rubro = await _mediator.Send(query);

            if (rubro == null)
            {
                return NotFound(new { error = $"Rubro con ID {id} no encontrado" });
            }

            return Ok(rubro);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el rubro con ID {RubroId}", id);
            return StatusCode(500, new { error = "Error al obtener el rubro", details = ex.Message });
        }
    }

    /// <summary>
    /// Crea un nuevo rubro
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<RubroDto>> Create([FromBody] CreateRubroCommand command)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(command.Descripcion))
            {
                return BadRequest(new { error = "La descripción del rubro es requerida" });
            }

            var rubro = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = rubro.RubroId }, rubro);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el rubro");
            return StatusCode(500, new { error = "Error al crear el rubro", details = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza un rubro existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<RubroDto>> Update(int id, [FromBody] UpdateRubroCommand command)
    {
        try
        {
            if (id != command.RubroId)
            {
                return BadRequest(new { error = "El ID de la URL no coincide con el ID del comando" });
            }

            if (string.IsNullOrWhiteSpace(command.Descripcion))
            {
                return BadRequest(new { error = "La descripción del rubro es requerida" });
            }

            var rubro = await _mediator.Send(command);

            if (rubro == null)
            {
                return NotFound(new { error = $"Rubro con ID {id} no encontrado" });
            }

            return Ok(rubro);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Rubro con ID {RubroId} no encontrado", id);
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el rubro con ID {RubroId}", id);
            return StatusCode(500, new { error = "Error al actualizar el rubro", details = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un rubro
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteRubroCommand { RubroId = id };
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound(new { error = $"Rubro con ID {id} no encontrado" });
            }

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Rubro con ID {RubroId} no encontrado", id);
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el rubro con ID {RubroId}", id);
            return StatusCode(500, new { error = "Error al eliminar el rubro", details = ex.Message });
        }
    }
}
