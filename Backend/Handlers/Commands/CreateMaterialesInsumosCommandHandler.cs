using AutoMapper;
using Backend.Commands.MaterialesInsumos;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateMaterialesInsumosCommandHandler : IRequestHandler<CreateMaterialesInsumosCommand, MaterialesInsumosDto>
{
    private readonly IMaterialesInsumosRepository _repo;
    private readonly IMapper _mapper;
    public CreateMaterialesInsumosCommandHandler(IMaterialesInsumosRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<MaterialesInsumosDto> Handle(CreateMaterialesInsumosCommand request, CancellationToken cancellationToken)
    {
        var entity = new MaterialesInsumos { RubroId = request.RubroId, Materiales = request.Materiales, Total = request.Total, RagEstado = request.RagEstado, PeriodoNum = request.PeriodoNum, PeriodoTipo = request.PeriodoTipo, ActividadId = request.ActividadId };
        var created = await _repo.CreateAsync(entity);
        return _mapper.Map<MaterialesInsumosDto>(created);
    }
}
