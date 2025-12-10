using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.CronogramaTareas;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetCronogramaTareaByIdHandler : IRequestHandler<GetCronogramaTareaByIdQuery, CronogramaTareaDto?>
{
    private readonly ICronogramaTareaRepository _cronogramaTareaRepository;

    public GetCronogramaTareaByIdHandler(ICronogramaTareaRepository cronogramaTareaRepository)
    {
        _cronogramaTareaRepository = cronogramaTareaRepository;
    }

    public async Task<CronogramaTareaDto?> Handle(GetCronogramaTareaByIdQuery request, CancellationToken cancellationToken)
    {
        var cronogramaTarea = await _cronogramaTareaRepository.GetByIdAsync(request.CronogramaId);
        if (cronogramaTarea == null)
            return null;

        return new CronogramaTareaDto
        {
            CronogramaId = cronogramaTarea.CronogramaId,
            TareaId = cronogramaTarea.TareaId,
            DuracionMeses = cronogramaTarea.DuracionMeses,
            DuracionDias = cronogramaTarea.DuracionDias,
            FechaInicio = cronogramaTarea.FechaInicio,
            FechaFin = cronogramaTarea.FechaFin,
            TareaNombre = cronogramaTarea.Tarea?.Nombre
        };
    }
}

public class GetAllCronogramaTareasHandler : IRequestHandler<GetAllCronogramaTareasQuery, IEnumerable<CronogramaTareaDto>>
{
    private readonly ICronogramaTareaRepository _cronogramaTareaRepository;

    public GetAllCronogramaTareasHandler(ICronogramaTareaRepository cronogramaTareaRepository)
    {
        _cronogramaTareaRepository = cronogramaTareaRepository;
    }

    public async Task<IEnumerable<CronogramaTareaDto>> Handle(GetAllCronogramaTareasQuery request, CancellationToken cancellationToken)
    {
        var cronogramaTareas = await _cronogramaTareaRepository.GetAllAsync();

        return cronogramaTareas.Select(c => new CronogramaTareaDto
        {
            CronogramaId = c.CronogramaId,
            TareaId = c.TareaId,
            DuracionMeses = c.DuracionMeses,
            DuracionDias = c.DuracionDias,
            FechaInicio = c.FechaInicio,
            FechaFin = c.FechaFin,
            TareaNombre = c.Tarea?.Nombre
        });
    }
}

public class GetCronogramaTareasByTareaIdHandler : IRequestHandler<GetCronogramaTareasByTareaIdQuery, IEnumerable<CronogramaTareaDto>>
{
    private readonly ICronogramaTareaRepository _cronogramaTareaRepository;

    public GetCronogramaTareasByTareaIdHandler(ICronogramaTareaRepository cronogramaTareaRepository)
    {
        _cronogramaTareaRepository = cronogramaTareaRepository;
    }

    public async Task<IEnumerable<CronogramaTareaDto>> Handle(GetCronogramaTareasByTareaIdQuery request, CancellationToken cancellationToken)
    {
        var cronogramaTareas = await _cronogramaTareaRepository.GetByTareaIdAsync(request.TareaId);

        return cronogramaTareas.Select(c => new CronogramaTareaDto
        {
            CronogramaId = c.CronogramaId,
            TareaId = c.TareaId,
            DuracionMeses = c.DuracionMeses,
            DuracionDias = c.DuracionDias,
            FechaInicio = c.FechaInicio,
            FechaFin = c.FechaFin,
            TareaNombre = c.Tarea?.Nombre
        });
    }
}
