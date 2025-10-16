using Microsoft.AspNetCore.Mvc;
using MediatR;
using Backend.Commands.CadenasDeValor;
using Backend.Queries.CadenasDeValor;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CadenasDeValorController : ControllerBase
{
    private readonly IMediator _mediator;
    public CadenasDeValorController(IMediator mediator) { _mediator = mediator; }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetAllCadenasDeValorQuery()));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _mediator.Send(new GetCadenaDeValorByIdQuery(id));
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCadenaDeValorCommand command)
    {
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = created.CadenaDeValorId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCadenaDeValorCommand command)
    {
        if (id != command.CadenaDeValorId) return BadRequest("ID mismatch");
        var updated = await _mediator.Send(command);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await _mediator.Send(new DeleteCadenaDeValorCommand { CadenaDeValorId = id });
        return res ? NoContent() : NotFound();
    }
}
