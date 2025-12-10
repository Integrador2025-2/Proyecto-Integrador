using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Actividades;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetActividadByIdHandler : IRequestHandler<GetActividadByIdQuery, ActividadDto?>
{
    private readonly IActividadRepository _actividadRepository;

    public GetActividadByIdHandler(IActividadRepository actividadRepository)
    {
        _actividadRepository = actividadRepository;
    }

    public async Task<ActividadDto?> Handle(GetActividadByIdQuery request, CancellationToken cancellationToken)
    {
        var actividad = await _actividadRepository.GetByIdAsync(request.ActividadId);
        if (actividad == null)
            return null;

        return new ActividadDto
        {
            ActividadId = actividad.ActividadId,
            CadenaDeValorId = actividad.CadenaDeValorId,
            Nombre = actividad.Nombre,
            Descripcion = actividad.Descripcion,
            Justificacion = actividad.Justificacion,
            DuracionAnios = actividad.DuracionAnios,
            EspecificacionesTecnicas = actividad.EspecificacionesTecnicas,
            ValorUnitario = actividad.ValorUnitario,
            CadenaDeValorNombre = actividad.CadenaDeValor?.Nombre
        };
    }
}

public class GetAllActividadesHandler : IRequestHandler<GetAllActividadesQuery, IEnumerable<ActividadDto>>
{
    private readonly IActividadRepository _actividadRepository;

    public GetAllActividadesHandler(IActividadRepository actividadRepository)
    {
        _actividadRepository = actividadRepository;
    }

    public async Task<IEnumerable<ActividadDto>> Handle(GetAllActividadesQuery request, CancellationToken cancellationToken)
    {
        var actividades = await _actividadRepository.GetAllAsync();

        return actividades.Select(a => new ActividadDto
        {
            ActividadId = a.ActividadId,
            CadenaDeValorId = a.CadenaDeValorId,
            Nombre = a.Nombre,
            Descripcion = a.Descripcion,
            Justificacion = a.Justificacion,
            DuracionAnios = a.DuracionAnios,
            EspecificacionesTecnicas = a.EspecificacionesTecnicas,
            ValorUnitario = a.ValorUnitario,
            CadenaDeValorNombre = a.CadenaDeValor?.Nombre
        });
    }
}

public class GetActividadesByCadenaDeValorIdHandler : IRequestHandler<GetActividadesByCadenaDeValorIdQuery, IEnumerable<ActividadDto>>
{
    private readonly IActividadRepository _actividadRepository;

    public GetActividadesByCadenaDeValorIdHandler(IActividadRepository actividadRepository)
    {
        _actividadRepository = actividadRepository;
    }

    public async Task<IEnumerable<ActividadDto>> Handle(GetActividadesByCadenaDeValorIdQuery request, CancellationToken cancellationToken)
    {
        var actividades = await _actividadRepository.GetByCadenaDeValorIdAsync(request.CadenaDeValorId);

        return actividades.Select(a => new ActividadDto
        {
            ActividadId = a.ActividadId,
            CadenaDeValorId = a.CadenaDeValorId,
            Nombre = a.Nombre,
            Descripcion = a.Descripcion,
            Justificacion = a.Justificacion,
            DuracionAnios = a.DuracionAnios,
            EspecificacionesTecnicas = a.EspecificacionesTecnicas,
            ValorUnitario = a.ValorUnitario,
            CadenaDeValorNombre = a.CadenaDeValor?.Nombre
        });
    }
}
