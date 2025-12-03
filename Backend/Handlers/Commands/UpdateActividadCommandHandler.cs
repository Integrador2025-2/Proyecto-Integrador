using AutoMapper;
using Backend.Commands.Actividades;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class UpdateActividadCommandHandler : IRequestHandler<UpdateActividadCommand, ActividadDto?>
{
    private readonly IActividadRepository _actividadRepository;
    private readonly IMapper _mapper;
    public UpdateActividadCommandHandler(IActividadRepository actividadRepository, IMapper mapper)
    {
        _actividadRepository = actividadRepository;
        _mapper = mapper;
    }

    public async Task<ActividadDto?> Handle(UpdateActividadCommand request, CancellationToken cancellationToken)
    {
        var actividad = new Actividad
        {
            ActividadId = request.ActividadId,
            ProyectoId = request.ProyectoId,
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            Justificacion = request.Justificacion,
            TotalxAnios = request.TotalxAnios ?? new List<decimal>(),
            CantidadAnios = request.CantidadAnios,
            EspecificacionesTecnicas = request.EspecificacionesTecnicas,
            ValorUnitario = request.ValorUnitario
        };

        var updated = await _actividadRepository.UpdateAsync(actividad);
        return updated == null ? null : _mapper.Map<ActividadDto>(updated);
    }
}
