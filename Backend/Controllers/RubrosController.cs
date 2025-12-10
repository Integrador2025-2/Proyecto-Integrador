using MediatR;
using Microsoft.AspNetCore.Mvc;
using Backend.Models.DTOs;
using Backend.Queries.Rubros;
using Backend.Commands.Rubros;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RubrosController : ControllerBase
{
    private readonly IMediator _mediator;
    public RubrosController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<RubroDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllRubrosQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RubroDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetRubroByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<RubroDto>> Create([FromBody] CreateRubroCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.RubroId }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RubroDto>> Update(int id, [FromBody] UpdateRubroCommand command)
    {
        if (id != command.RubroId) return BadRequest();
        var result = await _mediator.Send(command);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteRubroCommand(id));
        if (!deleted) return NotFound();
        return NoContent();
    }
}
