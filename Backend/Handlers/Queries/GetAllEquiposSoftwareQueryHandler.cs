using AutoMapper;
using Backend.Queries.EquiposSoftware;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetAllEquiposSoftwareQueryHandler : IRequestHandler<GetAllEquiposSoftwareQuery, List<EquiposSoftwareDto>>
{
    private readonly IEquiposSoftwareRepository _repo;
    private readonly IMapper _mapper;
    public GetAllEquiposSoftwareQueryHandler(IEquiposSoftwareRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<List<EquiposSoftwareDto>> Handle(GetAllEquiposSoftwareQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetAllAsync();
        return _mapper.Map<List<EquiposSoftwareDto>>(items);
    }
}
