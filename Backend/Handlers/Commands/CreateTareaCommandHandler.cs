using MediatR;
using AutoMapper;
using Backend.Commands.Tareas;
using Backend.Models.DTOs;
using Backend.Infrastructure.Repositories;

namespace Backend.Handlers.Commands;

public class CreateTareaCommandHandler : IRequestHandler<CreateTareaCommand, TareaDto>
{
    private readonly ITareaRepository _repo;
    private readonly IMapper _mapper;

    public CreateTareaCommandHandler(ITareaRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<TareaDto> Handle(CreateTareaCommand request, CancellationToken cancellationToken)
    {
        var entity = new Backend.Models.Domain.Tarea
        {
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            Periodo = request.Periodo,
            Monto = request.Monto,
            ActividadId = request.ActividadId
        };

    var created = await _repo.CreateAsync(entity);
        return _mapper.Map<TareaDto>(created);
    }
}
