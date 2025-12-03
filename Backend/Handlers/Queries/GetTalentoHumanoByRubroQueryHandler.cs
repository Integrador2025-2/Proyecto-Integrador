using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.TalentoHumano;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetTalentoHumanoByRubroQueryHandler : IRequestHandler<GetTalentoHumanoByRubroQuery, List<TalentoHumanoDto>>
{
    private readonly ITalentoHumanoRepository _repo;
    private readonly IMapper _mapper;
    public GetTalentoHumanoByRubroQueryHandler(ITalentoHumanoRepository repo, IMapper mapper)
    {
        _repo = repo; _mapper = mapper;
    }

    public async Task<List<TalentoHumanoDto>> Handle(GetTalentoHumanoByRubroQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetByRubroIdAsync(request.RubroId);
        return _mapper.Map<List<TalentoHumanoDto>>(items);
    }
}
