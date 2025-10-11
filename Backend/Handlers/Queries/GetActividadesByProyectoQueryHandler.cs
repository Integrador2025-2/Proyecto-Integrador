using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Actividades;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetActividadesByProyectoQueryHandler : IRequestHandler<GetActividadesByProyectoQuery, List<ActividadDto>>
{
    private readonly IActividadRepository _actividadRepository;
    private readonly IMapper _mapper;
    public GetActividadesByProyectoQueryHandler(IActividadRepository actividadRepository, IMapper mapper)
    {
        _actividadRepository = actividadRepository;
        _mapper = mapper;
    }

    public async Task<List<ActividadDto>> Handle(GetActividadesByProyectoQuery request, CancellationToken cancellationToken)
    {
        var items = await _actividadRepository.GetByProyectoIdAsync(request.ProyectoId);
        return _mapper.Map<List<ActividadDto>>(items);
    }
}
