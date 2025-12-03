using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Actividades;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetActividadByIdQueryHandler : IRequestHandler<GetActividadByIdQuery, ActividadDto?>
{
    private readonly IActividadRepository _actividadRepository;
    private readonly IMapper _mapper;
    public GetActividadByIdQueryHandler(IActividadRepository actividadRepository, IMapper mapper)
    {
        _actividadRepository = actividadRepository;
        _mapper = mapper;
    }

    public async Task<ActividadDto?> Handle(GetActividadByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _actividadRepository.GetByIdAsync(request.ActividadId);
        return item == null ? null : _mapper.Map<ActividadDto>(item);
    }
}
