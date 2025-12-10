using AutoMapper;
using Backend.Commands.TalentoHumanoTareas;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateTalentoHumanoTareaHandler : IRequestHandler<CreateTalentoHumanoTareaCommand, TalentoHumanoTareaDto>
{
    private readonly ITalentoHumanoTareaRepository _repository;
    private readonly IMapper _mapper;

    public CreateTalentoHumanoTareaHandler(ITalentoHumanoTareaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TalentoHumanoTareaDto> Handle(CreateTalentoHumanoTareaCommand request, CancellationToken cancellationToken)
    {
        var talentoHumanoTarea = new TalentoHumanoTarea
        {
            TalentoHumanoId = request.TalentoHumanoId,
            Tarea = request.Tarea,
            HorasAsignadas = request.HorasAsignadas,
            RolenTarea = request.RolenTarea,
            Observaciones = request.Observaciones,
            FechaAsignacion = request.FechaAsignacion
        };

        var created = await _repository.CreateAsync(talentoHumanoTarea);
        return _mapper.Map<TalentoHumanoTareaDto>(created);
    }
}

public class UpdateTalentoHumanoTareaHandler : IRequestHandler<UpdateTalentoHumanoTareaCommand, TalentoHumanoTareaDto>
{
    private readonly ITalentoHumanoTareaRepository _repository;
    private readonly IMapper _mapper;

    public UpdateTalentoHumanoTareaHandler(ITalentoHumanoTareaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TalentoHumanoTareaDto> Handle(UpdateTalentoHumanoTareaCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.TalentoHumanoTareasId);
        if (existing == null)
        {
            throw new KeyNotFoundException($"TalentoHumanoTarea with ID {request.TalentoHumanoTareasId} not found.");
        }

        existing.TalentoHumanoId = request.TalentoHumanoId;
        existing.Tarea = request.Tarea;
        existing.HorasAsignadas = request.HorasAsignadas;
        existing.RolenTarea = request.RolenTarea;
        existing.Observaciones = request.Observaciones;
        existing.FechaAsignacion = request.FechaAsignacion;

        var updated = await _repository.UpdateAsync(existing);
        return _mapper.Map<TalentoHumanoTareaDto>(updated);
    }
}

public class DeleteTalentoHumanoTareaHandler : IRequestHandler<DeleteTalentoHumanoTareaCommand>
{
    private readonly ITalentoHumanoTareaRepository _repository;

    public DeleteTalentoHumanoTareaHandler(ITalentoHumanoTareaRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteTalentoHumanoTareaCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.TalentoHumanoTareasId);
    }
}
