using Backend.Commands.Contratacion;
using Backend.Queries.Contratacion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/contrataciones")]
public class ContratacionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ContratacionController> _logger;

    public ContratacionController(IMediator mediator, ILogger<ContratacionController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllContratacionesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetContratacionByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (result == null)
        {
            return NotFound($"Contratacion with ID {id} not found.");
        }
        
        return Ok(result);
    }

    [HttpGet("categoria/{categoria}")]
    public async Task<IActionResult> GetByCategoria(string categoria)
    {
        var query = new GetContratacionesByCategoriaQuery(categoria);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContratacionCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.NivelGestion))
        {
            return BadRequest("NivelGestion is required.");
        }

        if (string.IsNullOrWhiteSpace(command.Categoria))
        {
            return BadRequest("Categoria is required.");
        }

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.ContratacionId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateContratacionCommand command)
    {
        if (id != command.ContratacionId)
        {
            return BadRequest("ID mismatch.");
        }

        if (string.IsNullOrWhiteSpace(command.NivelGestion))
        {
            return BadRequest("NivelGestion is required.");
        }

        if (string.IsNullOrWhiteSpace(command.Categoria))
        {
            return BadRequest("Categoria is required.");
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
        var command = new DeleteContratacionCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}
