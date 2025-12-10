using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.MaterialesInsumos;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetMaterialesInsumosByRubroQueryHandler : IRequestHandler<GetMaterialesInsumosByRubroQuery, List<MaterialesInsumosDto>>
{
    private readonly IMaterialesInsumosRepository _repo;
    private readonly IMapper _mapper;
    public GetMaterialesInsumosByRubroQueryHandler(IMaterialesInsumosRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<List<MaterialesInsumosDto>> Handle(GetMaterialesInsumosByRubroQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetByRubroIdAsync(request.RubroId);
        return _mapper.Map<List<MaterialesInsumosDto>>(items);
    }
}
