using Backend.Commands.Actividades;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateActividadHandler : IRequestHandler<CreateActividadCommand, ActividadDto>
{
    private readonly IActividadRepository _actividadRepository;

    public CreateActividadHandler(IActividadRepository actividadRepository)
    {
        _actividadRepository = actividadRepository;
    }

    public async Task<ActividadDto> Handle(CreateActividadCommand request, CancellationToken cancellationToken)
    {
        var actividad = new Actividad
        {
            CadenaDeValorId = request.CadenaDeValorId,
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            Justificacion = request.Justificacion,
            DuracionAnios = request.DuracionAnios,
            EspecificacionesTecnicas = request.EspecificacionesTecnicas,
            ValorUnitario = request.ValorUnitario
        };

        var createdActividad = await _actividadRepository.CreateAsync(actividad);

        return new ActividadDto
        {
            ActividadId = createdActividad.ActividadId,
            CadenaDeValorId = createdActividad.CadenaDeValorId,
            Nombre = createdActividad.Nombre,
            Descripcion = createdActividad.Descripcion,
            Justificacion = createdActividad.Justificacion,
            DuracionAnios = createdActividad.DuracionAnios,
            EspecificacionesTecnicas = createdActividad.EspecificacionesTecnicas,
            ValorUnitario = createdActividad.ValorUnitario
        };
    }
}

public class UpdateActividadHandler : IRequestHandler<UpdateActividadCommand, ActividadDto>
{
    private readonly IActividadRepository _actividadRepository;

    public UpdateActividadHandler(IActividadRepository actividadRepository)
    {
        _actividadRepository = actividadRepository;
    }

    public async Task<ActividadDto> Handle(UpdateActividadCommand request, CancellationToken cancellationToken)
    {
        var actividad = await _actividadRepository.GetByIdAsync(request.ActividadId);
        if (actividad == null)
            throw new KeyNotFoundException($"Actividad with ID {request.ActividadId} not found");

        actividad.Nombre = request.Nombre;
        actividad.Descripcion = request.Descripcion;
        actividad.Justificacion = request.Justificacion;
        actividad.DuracionAnios = request.DuracionAnios;
        actividad.EspecificacionesTecnicas = request.EspecificacionesTecnicas;
        actividad.ValorUnitario = request.ValorUnitario;

        var updatedActividad = await _actividadRepository.UpdateAsync(actividad);

        return new ActividadDto
        {
            ActividadId = updatedActividad.ActividadId,
            CadenaDeValorId = updatedActividad.CadenaDeValorId,
            Nombre = updatedActividad.Nombre,
            Descripcion = updatedActividad.Descripcion,
            Justificacion = updatedActividad.Justificacion,
            DuracionAnios = updatedActividad.DuracionAnios,
            EspecificacionesTecnicas = updatedActividad.EspecificacionesTecnicas,
            ValorUnitario = updatedActividad.ValorUnitario
        };
    }
}

public class DeleteActividadHandler : IRequestHandler<DeleteActividadCommand, bool>
{
    private readonly IActividadRepository _actividadRepository;

    public DeleteActividadHandler(IActividadRepository actividadRepository)
    {
        _actividadRepository = actividadRepository;
    }

    public async Task<bool> Handle(DeleteActividadCommand request, CancellationToken cancellationToken)
    {
        var exists = await _actividadRepository.ExistsAsync(request.ActividadId);
        if (!exists)
            return false;

        await _actividadRepository.DeleteAsync(request.ActividadId);
        return true;
    }
}
