using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.ActxEntidad;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetAllActxEntidadQueryHandler : IRequestHandler<GetAllActxEntidadQuery, List<ActxEntidadDto>>
{
    private readonly IActXEntidadRepository _repo;
    private readonly IMapper _mapper;

    public GetAllActxEntidadQueryHandler(IActXEntidadRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<ActxEntidadDto>> Handle(GetAllActxEntidadQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetAllAsync(request.ActividadId);
        return _mapper.Map<List<ActxEntidadDto>>(items);
    }
}
