using AutoMapper;
using Backend.Queries.MaterialesInsumos;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetMaterialesInsumosByIdQueryHandler : IRequestHandler<GetMaterialesInsumosByIdQuery, MaterialesInsumosDto?>
{
    private readonly IMaterialesInsumosRepository _repo;
    private readonly IMapper _mapper;
    public GetMaterialesInsumosByIdQueryHandler(IMaterialesInsumosRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<MaterialesInsumosDto?> Handle(GetMaterialesInsumosByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.Id);
        return item == null ? null : _mapper.Map<MaterialesInsumosDto>(item);
    }
}
