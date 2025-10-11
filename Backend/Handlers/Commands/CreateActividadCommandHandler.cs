using AutoMapper;
using Backend.Commands.Actividades;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateActividadCommandHandler : IRequestHandler<CreateActividadCommand, ActividadDto>
{
    private readonly IActividadRepository _actividadRepository;
    private readonly IMapper _mapper;
    public CreateActividadCommandHandler(IActividadRepository actividadRepository, IMapper mapper)
    {
        _actividadRepository = actividadRepository;
        _mapper = mapper;
    }

    public async Task<ActividadDto> Handle(CreateActividadCommand request, CancellationToken cancellationToken)
    {
        var actividad = new Actividad
        {
            ProyectoId = request.ProyectoId,
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            Justificacion = request.Justificacion,
            TotalxAnios = request.TotalxAnios ?? new List<decimal>(),
            CantidadAnios = request.CantidadAnios,
            EspecificacionesTecnicas = request.EspecificacionesTecnicas,
            ValorUnitario = request.ValorUnitario
        };

        var created = await _actividadRepository.CreateAsync(actividad);
        return _mapper.Map<ActividadDto>(created);
    }
}
