using AutoMapper;
using Backend.Models.DTOs;
using Backend.Queries.MaterialesInsumos;
using Backend.Infrastructure.Repositories;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetAllMaterialesInsumosQueryHandler : IRequestHandler<GetAllMaterialesInsumosQuery, List<MaterialesInsumosDto>>
{
    private readonly IMaterialesInsumosRepository _repo;
    private readonly IMapper _mapper;

    public GetAllMaterialesInsumosQueryHandler(IMaterialesInsumosRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<MaterialesInsumosDto>> Handle(GetAllMaterialesInsumosQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetAllAsync();
        return _mapper.Map<List<MaterialesInsumosDto>>(items);
    }
}
