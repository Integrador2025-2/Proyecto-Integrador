using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Recursos;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetRecursoByIdHandler : IRequestHandler<GetRecursoByIdQuery, RecursoDto?>
{
    private readonly IRecursoRepository _recursoRepository;

    public GetRecursoByIdHandler(IRecursoRepository recursoRepository)
    {
        _recursoRepository = recursoRepository;
    }

    public async Task<RecursoDto?> Handle(GetRecursoByIdQuery request, CancellationToken cancellationToken)
    {
        var recurso = await _recursoRepository.GetByIdAsync(request.RecursoId);
        if (recurso == null)
            return null;

        return new RecursoDto
        {
            RecursoId = recurso.RecursoId,
            ActividadId = recurso.ActividadId,
            EntidadId = recurso.EntidadId,
            RubroId = recurso.RubroId,
            TipoRecurso = recurso.TipoRecurso,
            MontoEfectivo = recurso.MontoEfectivo,
            MontoEspecie = recurso.MontoEspecie,
            Descripcion = recurso.Descripcion,
            ActividadNombre = recurso.Actividad?.Nombre,
            EntidadNombre = recurso.Entidad?.Nombre,
            RubroDescripcion = recurso.Rubro?.Descripcion
        };
    }
}

public class GetAllRecursosHandler : IRequestHandler<GetAllRecursosQuery, IEnumerable<RecursoDto>>
{
    private readonly IRecursoRepository _recursoRepository;

    public GetAllRecursosHandler(IRecursoRepository recursoRepository)
    {
        _recursoRepository = recursoRepository;
    }

    public async Task<IEnumerable<RecursoDto>> Handle(GetAllRecursosQuery request, CancellationToken cancellationToken)
    {
        var recursos = await _recursoRepository.GetAllAsync();

        return recursos.Select(r => new RecursoDto
        {
            RecursoId = r.RecursoId,
            ActividadId = r.ActividadId,
            EntidadId = r.EntidadId,
            RubroId = r.RubroId,
            TipoRecurso = r.TipoRecurso,
            MontoEfectivo = r.MontoEfectivo,
            MontoEspecie = r.MontoEspecie,
            Descripcion = r.Descripcion,
            ActividadNombre = r.Actividad?.Nombre,
            EntidadNombre = r.Entidad?.Nombre,
            RubroDescripcion = r.Rubro?.Descripcion
        });
    }
}

public class GetRecursosByActividadIdHandler : IRequestHandler<GetRecursosByActividadIdQuery, IEnumerable<RecursoDto>>
{
    private readonly IRecursoRepository _recursoRepository;

    public GetRecursosByActividadIdHandler(IRecursoRepository recursoRepository)
    {
        _recursoRepository = recursoRepository;
    }

    public async Task<IEnumerable<RecursoDto>> Handle(GetRecursosByActividadIdQuery request, CancellationToken cancellationToken)
    {
        var recursos = await _recursoRepository.GetByActividadIdAsync(request.ActividadId);

        return recursos.Select(r => new RecursoDto
        {
            RecursoId = r.RecursoId,
            ActividadId = r.ActividadId,
            EntidadId = r.EntidadId,
            RubroId = r.RubroId,
            TipoRecurso = r.TipoRecurso,
            MontoEfectivo = r.MontoEfectivo,
            MontoEspecie = r.MontoEspecie,
            Descripcion = r.Descripcion,
            ActividadNombre = r.Actividad?.Nombre,
            EntidadNombre = r.Entidad?.Nombre,
            RubroDescripcion = r.Rubro?.Descripcion
        });
    }
}
