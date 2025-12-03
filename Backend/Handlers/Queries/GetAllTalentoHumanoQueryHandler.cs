using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.TalentoHumano;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetAllTalentoHumanoQueryHandler : IRequestHandler<GetAllTalentoHumanoQuery, List<TalentoHumanoDto>>
{
    private readonly ITalentoHumanoRepository _repo;
    private readonly IMapper _mapper;
    public GetAllTalentoHumanoQueryHandler(ITalentoHumanoRepository repo, IMapper mapper)
    {
        _repo = repo; _mapper = mapper;
    }

    public async Task<List<TalentoHumanoDto>> Handle(GetAllTalentoHumanoQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetAllAsync();
        return _mapper.Map<List<TalentoHumanoDto>>(items);
    }
}
