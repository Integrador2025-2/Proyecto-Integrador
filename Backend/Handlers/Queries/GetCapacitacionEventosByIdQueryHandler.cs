using AutoMapper;
using Backend.Queries.CapacitacionEventos;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetCapacitacionEventosByIdQueryHandler : IRequestHandler<GetCapacitacionEventosByIdQuery, CapacitacionEventosDto?>
{
    private readonly ICapacitacionEventosRepository _repo;
    private readonly IMapper _mapper;
    public GetCapacitacionEventosByIdQueryHandler(ICapacitacionEventosRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<CapacitacionEventosDto?> Handle(GetCapacitacionEventosByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.Id);
        return item == null ? null : _mapper.Map<CapacitacionEventosDto>(item);
    }
}
