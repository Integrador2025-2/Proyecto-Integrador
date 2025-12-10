using AutoMapper;
using Backend.Queries.GastosViaje;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetAllGastosViajeQueryHandler : IRequestHandler<GetAllGastosViajeQuery, List<GastosViajeDto>>
{
    private readonly IGastosViajeRepository _repo;
    private readonly IMapper _mapper;
    public GetAllGastosViajeQueryHandler(IGastosViajeRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<List<GastosViajeDto>> Handle(GetAllGastosViajeQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetAllAsync();
        return _mapper.Map<List<GastosViajeDto>>(items);
    }
}
