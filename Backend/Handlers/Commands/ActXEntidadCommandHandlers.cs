using Backend.Commands.ActxEntidad;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateActXEntidadHandler : IRequestHandler<CreateActXEntidadCommand, ActXEntidadDto>
{
    private readonly IActXEntidadRepository _actXEntidadRepository;

    public CreateActXEntidadHandler(IActXEntidadRepository actXEntidadRepository)
    {
        _actXEntidadRepository = actXEntidadRepository;
    }

    public async Task<ActXEntidadDto> Handle(CreateActXEntidadCommand request, CancellationToken cancellationToken)
    {
        var actXEntidad = new ActXEntidad
        {
            ActividadId = request.ActividadId,
            EntidadId = request.EntidadId,
            Efectivo = request.Efectivo,
            Especie = request.Especie,
            TotalContribucion = request.TotalContribucion
        };

        var created = await _actXEntidadRepository.CreateAsync(actXEntidad);

        return new ActXEntidadDto
        {
            Id = created.Id,
            ActividadId = created.ActividadId,
            EntidadId = created.EntidadId,
            Efectivo = created.Efectivo,
            Especie = created.Especie,
            TotalContribucion = created.TotalContribucion
        };
    }
}

public class UpdateActXEntidadHandler : IRequestHandler<UpdateActXEntidadCommand, ActXEntidadDto>
{
    private readonly IActXEntidadRepository _actXEntidadRepository;

    public UpdateActXEntidadHandler(IActXEntidadRepository actXEntidadRepository)
    {
        _actXEntidadRepository = actXEntidadRepository;
    }

    public async Task<ActXEntidadDto> Handle(UpdateActXEntidadCommand request, CancellationToken cancellationToken)
    {
        var actXEntidad = await _actXEntidadRepository.GetByIdAsync(request.Id);
        if (actXEntidad == null)
            throw new KeyNotFoundException($"ActXEntidad with ID {request.Id} not found");

        actXEntidad.Efectivo = request.Efectivo;
        actXEntidad.Especie = request.Especie;
        actXEntidad.TotalContribucion = request.TotalContribucion;

        var updated = await _actXEntidadRepository.UpdateAsync(actXEntidad);

        return new ActXEntidadDto
        {
            Id = updated.Id,
            ActividadId = updated.ActividadId,
            EntidadId = updated.EntidadId,
            Efectivo = updated.Efectivo,
            Especie = updated.Especie,
            TotalContribucion = updated.TotalContribucion
        };
    }
}

public class DeleteActXEntidadHandler : IRequestHandler<DeleteActXEntidadCommand, bool>
{
    private readonly IActXEntidadRepository _actXEntidadRepository;

    public DeleteActXEntidadHandler(IActXEntidadRepository actXEntidadRepository)
    {
        _actXEntidadRepository = actXEntidadRepository;
    }

    public async Task<bool> Handle(DeleteActXEntidadCommand request, CancellationToken cancellationToken)
    {
        var exists = await _actXEntidadRepository.ExistsAsync(request.Id);
        if (!exists)
            return false;

        await _actXEntidadRepository.DeleteAsync(request.Id);
        return true;
    }
}
