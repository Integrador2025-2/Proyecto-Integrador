using Backend.Commands.RemuneracionPorAnio;
using Backend.Queries.RemuneracionPorAnio;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/remuneracionporanio")]
public class RemuneracionPorAnioController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RemuneracionPorAnioController> _logger;

    public RemuneracionPorAnioController(IMediator mediator, ILogger<RemuneracionPorAnioController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllRemuneracionPorAnioQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetRemuneracionPorAnioByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (result == null)
        {
            return NotFound($"RemuneracionPorAnio with ID {id} not found.");
        }
        
        return Ok(result);
    }

    [HttpGet("talentohumano/{talentoHumanoId}")]
    public async Task<IActionResult> GetByTalentoHumanoId(int talentoHumanoId)
    {
        var query = new GetRemuneracionPorAnioByTalentoHumanoIdQuery(talentoHumanoId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("anio/{anio}")]
    public async Task<IActionResult> GetByAnio(int anio)
    {
        var query = new GetRemuneracionPorAnioByAnioQuery(anio);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRemuneracionPorAnioCommand command)
    {
        if (command.TalentoHumanoId <= 0)
        {
            return BadRequest("TalentoHumanoId must be greater than 0.");
        }

        if (command.Anio <= 0)
        {
            return BadRequest("Anio must be greater than 0.");
        }

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.RemuneracionPorAnioId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRemuneracionPorAnioCommand command)
    {
        if (id != command.RemuneracionPorAnioId)
        {
            return BadRequest("ID mismatch.");
        }

        if (command.TalentoHumanoId <= 0)
        {
            return BadRequest("TalentoHumanoId must be greater than 0.");
        }

        if (command.Anio <= 0)
        {
            return BadRequest("Anio must be greater than 0.");
        }

        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteRemuneracionPorAnioCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}
