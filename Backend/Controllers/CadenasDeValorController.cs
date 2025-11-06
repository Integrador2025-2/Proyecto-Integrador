using Backend.Commands.CadenasDeValor;
using Backend.Models.DTOs;
using Backend.Queries.CadenasDeValor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CadenasDeValorController : ControllerBase
{
    private readonly IMediator _mediator;

    public CadenasDeValorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CadenaDeValorDto>>> GetAll()
    {
        var query = new GetAllCadenasDeValorQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CadenaDeValorDto>> GetById(int id)
    {
        var query = new GetCadenaDeValorByIdQuery { CadenaDeValorId = id };
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound();
            
        return Ok(result);
    }

    [HttpGet("objetivo/{objetivoId}")]
    public async Task<ActionResult<IEnumerable<CadenaDeValorDto>>> GetByObjetivoId(int objetivoId)
    {
        var query = new GetCadenasDeValorByObjetivoIdQuery { ObjetivoId = objetivoId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CadenaDeValorDto>> Create([FromBody] CreateCadenaDeValorCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.CadenaDeValorId }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CadenaDeValorDto>> Update(int id, [FromBody] UpdateCadenaDeValorCommand command)
    {
        if (id != command.CadenaDeValorId)
            return BadRequest();

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteCadenaDeValorCommand { CadenaDeValorId = id };
        var result = await _mediator.Send(command);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}
