using MediatR;
using Microsoft.AspNetCore.Mvc;
using Backend.Models.DTOs;
using Backend.Queries.Actividades;
using Backend.Commands.Actividades;
using Backend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Json;
using Backend.Models.Domain;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActividadesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public ActividadesController(IMediator mediator, ApplicationDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _mediator = mediator;
        _context = context;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<List<ActividadDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllActividadesQuery());
        return Ok(result);
    }

    [HttpGet("proyecto/{proyectoId}")]
    public async Task<ActionResult<List<ActividadDto>>> GetByProyecto(int proyectoId)
    {
        var result = await _mediator.Send(new GetActividadesByProyectoQuery(proyectoId));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActividadDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetActividadByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ActividadDto>> Create([FromBody] CreateActividadCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.ActividadId }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ActividadDto>> Update(int id, [FromBody] UpdateActividadCommand command)
    {
        if (id != command.ActividadId) return BadRequest();
        var result = await _mediator.Send(command);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteActividadCommand(id));
        if (!deleted) return NotFound();
        return NoContent();
    }

    [HttpPost("{id}/presupuesto-calculate")]
    public async Task<IActionResult> CalcularPresupuesto(int id)
    {
        // 1. Obtener actividad y rubros relacionados de la BD
        var actividad = await _context.Set<Actividad>()
            .Include(a => a.TalentoHumano)
            .Include(a => a.EquiposSoftware)
            .FirstOrDefaultAsync(a => a.ActividadId == id);

        if (actividad == null) return NotFound();

        // 2. Preparar datos para n8n
        var payload = new
        {
            ActividadId = actividad.ActividadId,
            Nombre = actividad.Nombre,
            Descripcion = actividad.Descripcion,
            ValorUnitario = actividad.ValorUnitario,
            TotalxAnios = actividad.TotalxAnios,
            CantidadAnios = actividad.CantidadAnios,
            EspecificacionesTecnicas = actividad.EspecificacionesTecnicas,
            TalentoHumano = actividad.TalentoHumano?.Select(r => new {
                r.TalentoHumanoId,
                r.CargoEspecifico,
                r.Semanas,
                r.Total,
                r.RagEstado,
                r.PeriodoNum,
                r.PeriodoTipo
            }) ?? Enumerable.Empty<object>(),
            EquiposSoftware = actividad.EquiposSoftware?.Select(e => new {
                e.EquiposSoftwareId,
                e.EspecificacionesTecnicas,
                e.Cantidad,
                e.Total,
                e.RagEstado,
                e.PeriodoNum,
                e.PeriodoTipo
            }) ?? Enumerable.Empty<object>()
        };

        // 3. Llamar al webhook de n8n (usa IHttpClientFactory)
    var n8nUrl = _configuration["N8N:WebhookUrl"] ?? "https://tu-n8n.com/webhook/calculo-presupuesto";
    var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync(n8nUrl, payload);

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());

        var resultado = await response.Content.ReadFromJsonAsync<object?>();
        return Ok(resultado);
    }
}
