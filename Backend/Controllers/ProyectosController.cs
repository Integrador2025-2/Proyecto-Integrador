using MediatR;
using Backend.Models.DTOs;
using Backend.Commands.Proyectos;
using Backend.Queries.Proyectos;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProyectosController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProyectosController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<List<ProyectoDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllProyectosQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProyectoDto>> GetById(int id)
        {
            var result = await _mediator.Send(new GetProyectoByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ProyectoDto>> Create([FromBody] CreateProyectoCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.ProyectoId }, result);
        }
    }
}