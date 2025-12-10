using AutoMapper;
using Backend.Queries.CapacitacionEventos;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetAllCapacitacionEventosQueryHandler : IRequestHandler<GetAllCapacitacionEventosQuery, List<CapacitacionEventosDto>>
{
    private readonly ICapacitacionEventosRepository _repo;
    private readonly IMapper _mapper;
    public GetAllCapacitacionEventosQueryHandler(ICapacitacionEventosRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<List<CapacitacionEventosDto>> Handle(GetAllCapacitacionEventosQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetAllAsync();
        return _mapper.Map<List<CapacitacionEventosDto>>(items);
    }
}
