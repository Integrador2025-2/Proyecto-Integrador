using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.ActxEntidad;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetActXEntidadByIdHandler : IRequestHandler<GetActXEntidadByIdQuery, ActXEntidadDto?>
{
    private readonly IActXEntidadRepository _actXEntidadRepository;

    public GetActXEntidadByIdHandler(IActXEntidadRepository actXEntidadRepository)
    {
        _actXEntidadRepository = actXEntidadRepository;
    }

    public async Task<ActXEntidadDto?> Handle(GetActXEntidadByIdQuery request, CancellationToken cancellationToken)
    {
        var actXEntidad = await _actXEntidadRepository.GetByIdAsync(request.Id);
        if (actXEntidad == null)
            return null;

        return new ActXEntidadDto
        {
            Id = actXEntidad.Id,
            ActividadId = actXEntidad.ActividadId,
            EntidadId = actXEntidad.EntidadId,
            Efectivo = actXEntidad.Efectivo,
            Especie = actXEntidad.Especie,
            TotalContribucion = actXEntidad.TotalContribucion,
            ActividadNombre = actXEntidad.Actividad?.Nombre,
            EntidadNombre = actXEntidad.Entidad?.Nombre
        };
    }
}

public class GetAllActXEntidadesHandler : IRequestHandler<GetAllActXEntidadesQuery, IEnumerable<ActXEntidadDto>>
{
    private readonly IActXEntidadRepository _actXEntidadRepository;

    public GetAllActXEntidadesHandler(IActXEntidadRepository actXEntidadRepository)
    {
        _actXEntidadRepository = actXEntidadRepository;
    }

    public async Task<IEnumerable<ActXEntidadDto>> Handle(GetAllActXEntidadesQuery request, CancellationToken cancellationToken)
    {
        var actXEntidades = await _actXEntidadRepository.GetAllAsync();

        return actXEntidades.Select(a => new ActXEntidadDto
        {
            Id = a.Id,
            ActividadId = a.ActividadId,
            EntidadId = a.EntidadId,
            Efectivo = a.Efectivo,
            Especie = a.Especie,
            TotalContribucion = a.TotalContribucion,
            ActividadNombre = a.Actividad?.Nombre,
            EntidadNombre = a.Entidad?.Nombre
        });
    }
}

public class GetActXEntidadesByActividadIdHandler : IRequestHandler<GetActXEntidadesByActividadIdQuery, IEnumerable<ActXEntidadDto>>
{
    private readonly IActXEntidadRepository _actXEntidadRepository;

    public GetActXEntidadesByActividadIdHandler(IActXEntidadRepository actXEntidadRepository)
    {
        _actXEntidadRepository = actXEntidadRepository;
    }

    public async Task<IEnumerable<ActXEntidadDto>> Handle(GetActXEntidadesByActividadIdQuery request, CancellationToken cancellationToken)
    {
        var actXEntidades = await _actXEntidadRepository.GetByActividadIdAsync(request.ActividadId);

        return actXEntidades.Select(a => new ActXEntidadDto
        {
            Id = a.Id,
            ActividadId = a.ActividadId,
            EntidadId = a.EntidadId,
            Efectivo = a.Efectivo,
            Especie = a.Especie,
            TotalContribucion = a.TotalContribucion,
            ActividadNombre = a.Actividad?.Nombre,
            EntidadNombre = a.Entidad?.Nombre
        });
    }
}

public class GetActXEntidadesByEntidadIdHandler : IRequestHandler<GetActXEntidadesByEntidadIdQuery, IEnumerable<ActXEntidadDto>>
{
    private readonly IActXEntidadRepository _actXEntidadRepository;

    public GetActXEntidadesByEntidadIdHandler(IActXEntidadRepository actXEntidadRepository)
    {
        _actXEntidadRepository = actXEntidadRepository;
    }

    public async Task<IEnumerable<ActXEntidadDto>> Handle(GetActXEntidadesByEntidadIdQuery request, CancellationToken cancellationToken)
    {
        var actXEntidades = await _actXEntidadRepository.GetByEntidadIdAsync(request.EntidadId);

        return actXEntidades.Select(a => new ActXEntidadDto
        {
            Id = a.Id,
            ActividadId = a.ActividadId,
            EntidadId = a.EntidadId,
            Efectivo = a.Efectivo,
            Especie = a.Especie,
            TotalContribucion = a.TotalContribucion,
            ActividadNombre = a.Actividad?.Nombre,
            EntidadNombre = a.Entidad?.Nombre
        });
    }
}
