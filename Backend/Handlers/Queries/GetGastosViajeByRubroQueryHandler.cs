using AutoMapper;
using Backend.Queries.GastosViaje;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetGastosViajeByRubroQueryHandler : IRequestHandler<GetGastosViajeByRubroQuery, List<GastosViajeDto>>
{
    private readonly IGastosViajeRepository _repo;
    private readonly IMapper _mapper;
    public GetGastosViajeByRubroQueryHandler(IGastosViajeRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<List<GastosViajeDto>> Handle(GetGastosViajeByRubroQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetByRubroIdAsync(request.RubroId);
        return _mapper.Map<List<GastosViajeDto>>(items);
    }
}
