using Backend.Commands.GastosViaje;
using Backend.Models.DTOs;
using Backend.Queries.GastosViaje;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GastosViajeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<GastosViajeController> _logger;

    public GastosViajeController(IMediator mediator, ILogger<GastosViajeController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los gastos de viaje
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GastosViajeDto>>> GetAll()
    {
        try
        {
            var query = new GetAllGastosViajeQuery();
            var gastosViaje = await _mediator.Send(query);
            return Ok(gastosViaje);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los gastos de viaje");
            return StatusCode(500, new { error = "Error al obtener los gastos de viaje", details = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene un gasto de viaje por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<GastosViajeDto>> GetById(int id)
    {
        try
        {
            var query = new GetGastosViajeByIdQuery { GastosViajeId = id };
            var gastosViaje = await _mediator.Send(query);

            if (gastosViaje == null)
            {
                return NotFound(new { error = $"Gasto de viaje con ID {id} no encontrado" });
            }

            return Ok(gastosViaje);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el gasto de viaje con ID {GastosViajeId}", id);
            return StatusCode(500, new { error = "Error al obtener el gasto de viaje", details = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene un gasto de viaje por RecursoEspecificoId
    /// </summary>
    [HttpGet("recurso-especifico/{recursoEspecificoId}")]
    public async Task<ActionResult<GastosViajeDto>> GetByRecursoEspecificoId(int recursoEspecificoId)
    {
        try
        {
            var query = new GetGastosViajeByRecursoEspecificoIdQuery { RecursoEspecificoId = recursoEspecificoId };
            var gastosViaje = await _mediator.Send(query);

            if (gastosViaje == null)
            {
                return NotFound(new { error = $"Gasto de viaje para RecursoEspecifico {recursoEspecificoId} no encontrado" });
            }

            return Ok(gastosViaje);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el gasto de viaje para RecursoEspecifico {RecursoEspecificoId}", recursoEspecificoId);
            return StatusCode(500, new { error = "Error al obtener el gasto de viaje", details = ex.Message });
        }
    }

    /// <summary>
    /// Crea un nuevo gasto de viaje
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<GastosViajeDto>> Create([FromBody] CreateGastosViajeCommand command)
    {
        try
        {
            if (command.RecursoEspecificoId <= 0)
            {
                return BadRequest(new { error = "El RecursoEspecificoId es requerido" });
            }

            var gastosViaje = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = gastosViaje.GastosViajeId }, gastosViaje);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el gasto de viaje");
            return StatusCode(500, new { error = "Error al crear el gasto de viaje", details = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza un gasto de viaje existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<GastosViajeDto>> Update(int id, [FromBody] UpdateGastosViajeCommand command)
    {
        try
        {
            if (id != command.GastosViajeId)
            {
                return BadRequest(new { error = "El ID de la URL no coincide con el ID del comando" });
            }

            var gastosViaje = await _mediator.Send(command);

            if (gastosViaje == null)
            {
                return NotFound(new { error = $"Gasto de viaje con ID {id} no encontrado" });
            }

            return Ok(gastosViaje);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Gasto de viaje con ID {GastosViajeId} no encontrado", id);
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el gasto de viaje con ID {GastosViajeId}", id);
            return StatusCode(500, new { error = "Error al actualizar el gasto de viaje", details = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un gasto de viaje
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteGastosViajeCommand { GastosViajeId = id };
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound(new { error = $"Gasto de viaje con ID {id} no encontrado" });
            }

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Gasto de viaje con ID {GastosViajeId} no encontrado", id);
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el gasto de viaje con ID {GastosViajeId}", id);
            return StatusCode(500, new { error = "Error al eliminar el gasto de viaje", details = ex.Message });
        }
    }
}
