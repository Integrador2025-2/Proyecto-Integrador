using Backend.Commands.Tareas;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateTareaHandler : IRequestHandler<CreateTareaCommand, TareaDto>
{
    private readonly ITareaRepository _tareaRepository;

    public CreateTareaHandler(ITareaRepository tareaRepository)
    {
        _tareaRepository = tareaRepository;
    }

    public async Task<TareaDto> Handle(CreateTareaCommand request, CancellationToken cancellationToken)
    {
        var tarea = new Tarea
        {
            ActividadId = request.ActividadId,
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            Periodo = request.Periodo,
            Monto = request.Monto
        };

        var createdTarea = await _tareaRepository.CreateAsync(tarea);

        return new TareaDto
        {
            TareaId = createdTarea.TareaId,
            ActividadId = createdTarea.ActividadId,
            Nombre = createdTarea.Nombre,
            Descripcion = createdTarea.Descripcion,
            Periodo = createdTarea.Periodo,
            Monto = createdTarea.Monto
        };
    }
}

public class UpdateTareaHandler : IRequestHandler<UpdateTareaCommand, TareaDto>
{
    private readonly ITareaRepository _tareaRepository;

    public UpdateTareaHandler(ITareaRepository tareaRepository)
    {
        _tareaRepository = tareaRepository;
    }

    public async Task<TareaDto> Handle(UpdateTareaCommand request, CancellationToken cancellationToken)
    {
        var tarea = await _tareaRepository.GetByIdAsync(request.TareaId);
        if (tarea == null)
            throw new KeyNotFoundException($"Tarea with ID {request.TareaId} not found");

        tarea.Nombre = request.Nombre;
        tarea.Descripcion = request.Descripcion;
        tarea.Periodo = request.Periodo;
        tarea.Monto = request.Monto;

        var updatedTarea = await _tareaRepository.UpdateAsync(tarea);

        return new TareaDto
        {
            TareaId = updatedTarea.TareaId,
            ActividadId = updatedTarea.ActividadId,
            Nombre = updatedTarea.Nombre,
            Descripcion = updatedTarea.Descripcion,
            Periodo = updatedTarea.Periodo,
            Monto = updatedTarea.Monto
        };
    }
}

public class DeleteTareaHandler : IRequestHandler<DeleteTareaCommand, bool>
{
    private readonly ITareaRepository _tareaRepository;

    public DeleteTareaHandler(ITareaRepository tareaRepository)
    {
        _tareaRepository = tareaRepository;
    }

    public async Task<bool> Handle(DeleteTareaCommand request, CancellationToken cancellationToken)
    {
        var exists = await _tareaRepository.ExistsAsync(request.TareaId);
        if (!exists)
            return false;

        await _tareaRepository.DeleteAsync(request.TareaId);
        return true;
    }
}
