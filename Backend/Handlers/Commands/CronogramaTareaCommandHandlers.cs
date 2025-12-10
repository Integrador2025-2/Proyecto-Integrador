using Backend.Commands.CronogramaTareas;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateCronogramaTareaHandler : IRequestHandler<CreateCronogramaTareaCommand, CronogramaTareaDto>
{
    private readonly ICronogramaTareaRepository _cronogramaTareaRepository;

    public CreateCronogramaTareaHandler(ICronogramaTareaRepository cronogramaTareaRepository)
    {
        _cronogramaTareaRepository = cronogramaTareaRepository;
    }

    public async Task<CronogramaTareaDto> Handle(CreateCronogramaTareaCommand request, CancellationToken cancellationToken)
    {
        var cronogramaTarea = new CronogramaTarea
        {
            TareaId = request.TareaId,
            DuracionMeses = request.DuracionMeses,
            DuracionDias = request.DuracionDias,
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin
        };

        var created = await _cronogramaTareaRepository.CreateAsync(cronogramaTarea);

        return new CronogramaTareaDto
        {
            CronogramaId = created.CronogramaId,
            TareaId = created.TareaId,
            DuracionMeses = created.DuracionMeses,
            DuracionDias = created.DuracionDias,
            FechaInicio = created.FechaInicio,
            FechaFin = created.FechaFin
        };
    }
}

public class UpdateCronogramaTareaHandler : IRequestHandler<UpdateCronogramaTareaCommand, CronogramaTareaDto>
{
    private readonly ICronogramaTareaRepository _cronogramaTareaRepository;

    public UpdateCronogramaTareaHandler(ICronogramaTareaRepository cronogramaTareaRepository)
    {
        _cronogramaTareaRepository = cronogramaTareaRepository;
    }

    public async Task<CronogramaTareaDto> Handle(UpdateCronogramaTareaCommand request, CancellationToken cancellationToken)
    {
        var cronogramaTarea = await _cronogramaTareaRepository.GetByIdAsync(request.CronogramaId);
        if (cronogramaTarea == null)
            throw new KeyNotFoundException($"CronogramaTarea with ID {request.CronogramaId} not found");

        cronogramaTarea.DuracionMeses = request.DuracionMeses;
        cronogramaTarea.DuracionDias = request.DuracionDias;
        cronogramaTarea.FechaInicio = request.FechaInicio;
        cronogramaTarea.FechaFin = request.FechaFin;

        var updated = await _cronogramaTareaRepository.UpdateAsync(cronogramaTarea);

        return new CronogramaTareaDto
        {
            CronogramaId = updated.CronogramaId,
            TareaId = updated.TareaId,
            DuracionMeses = updated.DuracionMeses,
            DuracionDias = updated.DuracionDias,
            FechaInicio = updated.FechaInicio,
            FechaFin = updated.FechaFin
        };
    }
}

public class DeleteCronogramaTareaHandler : IRequestHandler<DeleteCronogramaTareaCommand, bool>
{
    private readonly ICronogramaTareaRepository _cronogramaTareaRepository;

    public DeleteCronogramaTareaHandler(ICronogramaTareaRepository cronogramaTareaRepository)
    {
        _cronogramaTareaRepository = cronogramaTareaRepository;
    }

    public async Task<bool> Handle(DeleteCronogramaTareaCommand request, CancellationToken cancellationToken)
    {
        var exists = await _cronogramaTareaRepository.ExistsAsync(request.CronogramaId);
        if (!exists)
            return false;

        await _cronogramaTareaRepository.DeleteAsync(request.CronogramaId);
        return true;
    }
}
