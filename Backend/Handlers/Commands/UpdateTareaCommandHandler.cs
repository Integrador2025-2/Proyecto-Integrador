using MediatR;
using AutoMapper;
using Backend.Commands.Tareas;
using Backend.Models.DTOs;
using Backend.Infrastructure.Repositories;

namespace Backend.Handlers.Commands;

public class UpdateTareaCommandHandler : IRequestHandler<UpdateTareaCommand, TareaDto>
{
    private readonly ITareaRepository _repo;
    private readonly IMapper _mapper;

    public UpdateTareaCommandHandler(ITareaRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<TareaDto> Handle(UpdateTareaCommand request, CancellationToken cancellationToken)
    {
    var existing = await _repo.GetByIdAsync(request.TareaId);
        if (existing == null) return null!;

        existing.Nombre = request.Nombre;
        existing.Descripcion = request.Descripcion;
        existing.Periodo = request.Periodo;
        existing.Monto = request.Monto;
        existing.ActividadId = request.ActividadId;

    var updated = await _repo.UpdateAsync(existing);
        return _mapper.Map<TareaDto>(updated);
    }
}
