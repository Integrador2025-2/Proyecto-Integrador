using AutoMapper;
using Backend.Queries.GastosViaje;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetGastosViajeByIdQueryHandler : IRequestHandler<GetGastosViajeByIdQuery, GastosViajeDto?>
{
    private readonly IGastosViajeRepository _repo;
    private readonly IMapper _mapper;
    public GetGastosViajeByIdQueryHandler(IGastosViajeRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<GastosViajeDto?> Handle(GetGastosViajeByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.Id);
        return item == null ? null : _mapper.Map<GastosViajeDto>(item);
    }
}
