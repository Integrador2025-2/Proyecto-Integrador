using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.MaterialesInsumos;
using Backend.Commands.MaterialesInsumos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaterialesInsumosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMaterialesInsumosRepository _repo;
    private readonly IMapper _mapper;

    public MaterialesInsumosController(IMediator mediator, IMaterialesInsumosRepository repo, IMapper mapper)
    {
        _mediator = mediator;
        _repo = repo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<MaterialesInsumosDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllMaterialesInsumosQuery());
        return Ok(result);
    }

    [HttpGet("rubro/{rubroId}")]
    public async Task<ActionResult<List<MaterialesInsumosDto>>> GetByRubro(int rubroId)
    {
        var items = await _repo.GetByRubroIdAsync(rubroId);
        return Ok(items.Select(x => _mapper.Map<MaterialesInsumosDto>(x)).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<MaterialesInsumosDto>> Create(CreateMaterialesInsumosCommand command)
    {
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = created.MaterialesInsumosId }, created);
    }
}
