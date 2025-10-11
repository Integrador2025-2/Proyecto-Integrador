using AutoMapper;
using Backend.Queries.CapacitacionEventos;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetCapacitacionEventosByRubroQueryHandler : IRequestHandler<GetCapacitacionEventosByRubroQuery, List<CapacitacionEventosDto>>
{
    private readonly ICapacitacionEventosRepository _repo;
    private readonly IMapper _mapper;
    public GetCapacitacionEventosByRubroQueryHandler(ICapacitacionEventosRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<List<CapacitacionEventosDto>> Handle(GetCapacitacionEventosByRubroQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetByRubroIdAsync(request.RubroId);
        return _mapper.Map<List<CapacitacionEventosDto>>(items);
    }
}
