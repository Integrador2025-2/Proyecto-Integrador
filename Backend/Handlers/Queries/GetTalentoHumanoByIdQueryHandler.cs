using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.TalentoHumano;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetTalentoHumanoByIdQueryHandler : IRequestHandler<GetTalentoHumanoByIdQuery, TalentoHumanoDto?>
{
    private readonly ITalentoHumanoRepository _repo;
    private readonly IMapper _mapper;
    public GetTalentoHumanoByIdQueryHandler(ITalentoHumanoRepository repo, IMapper mapper)
    {
        _repo = repo; _mapper = mapper;
    }

    public async Task<TalentoHumanoDto?> Handle(GetTalentoHumanoByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.TalentoHumanoId);
        return item == null ? null : _mapper.Map<TalentoHumanoDto>(item);
    }
}
