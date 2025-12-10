using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Tareas;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetTareaByIdHandler : IRequestHandler<GetTareaByIdQuery, TareaDto?>
{
    private readonly ITareaRepository _tareaRepository;

    public GetTareaByIdHandler(ITareaRepository tareaRepository)
    {
        _tareaRepository = tareaRepository;
    }

    public async Task<TareaDto?> Handle(GetTareaByIdQuery request, CancellationToken cancellationToken)
    {
        var tarea = await _tareaRepository.GetByIdAsync(request.TareaId);
        if (tarea == null)
            return null;

        return new TareaDto
        {
            TareaId = tarea.TareaId,
            ActividadId = tarea.ActividadId,
            Nombre = tarea.Nombre,
            Descripcion = tarea.Descripcion,
            Periodo = tarea.Periodo,
            Monto = tarea.Monto
        };
    }
}

public class GetAllTareasHandler : IRequestHandler<GetAllTareasQuery, IEnumerable<TareaDto>>
{
    private readonly ITareaRepository _tareaRepository;

    public GetAllTareasHandler(ITareaRepository tareaRepository)
    {
        _tareaRepository = tareaRepository;
    }

    public async Task<IEnumerable<TareaDto>> Handle(GetAllTareasQuery request, CancellationToken cancellationToken)
    {
        var tareas = await _tareaRepository.GetAllAsync();

        return tareas.Select(t => new TareaDto
        {
            TareaId = t.TareaId,
            ActividadId = t.ActividadId,
            Nombre = t.Nombre,
            Descripcion = t.Descripcion,
            Periodo = t.Periodo,
            Monto = t.Monto
        });
    }
}

public class GetTareasByActividadIdHandler : IRequestHandler<GetTareasByActividadIdQuery, IEnumerable<TareaDto>>
{
    private readonly ITareaRepository _tareaRepository;

    public GetTareasByActividadIdHandler(ITareaRepository tareaRepository)
    {
        _tareaRepository = tareaRepository;
    }

    public async Task<IEnumerable<TareaDto>> Handle(GetTareasByActividadIdQuery request, CancellationToken cancellationToken)
    {
        var tareas = await _tareaRepository.GetByActividadIdAsync(request.ActividadId);

        return tareas.Select(t => new TareaDto
        {
            TareaId = t.TareaId,
            ActividadId = t.ActividadId,
            Nombre = t.Nombre,
            Descripcion = t.Descripcion,
            Periodo = t.Periodo,
            Monto = t.Monto
        });
    }
}
