using AutoMapper;
using Backend.Queries.EquiposSoftware;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetEquiposSoftwareByRubroQueryHandler : IRequestHandler<GetEquiposSoftwareByRubroQuery, List<EquiposSoftwareDto>>
{
    private readonly IEquiposSoftwareRepository _repo;
    private readonly IMapper _mapper;
    public GetEquiposSoftwareByRubroQueryHandler(IEquiposSoftwareRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<List<EquiposSoftwareDto>> Handle(GetEquiposSoftwareByRubroQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetByRubroIdAsync(request.RubroId);
        return _mapper.Map<List<EquiposSoftwareDto>>(items);
    }
}
