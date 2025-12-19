using AutoMapper;
using Backend.Commands.CapacitacionEventos;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateCapacitacionEventosHandler : IRequestHandler<CreateCapacitacionEventosCommand, CapacitacionEventosDto>
{
    private readonly ICapacitacionEventosRepository _repository;
    private readonly IMapper _mapper;

    public CreateCapacitacionEventosHandler(ICapacitacionEventosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CapacitacionEventosDto> Handle(CreateCapacitacionEventosCommand request, CancellationToken cancellationToken)
    {
        var capacitacionEventos = new CapacitacionEventos
        {
            RecursoEspecificoId = request.RecursoEspecificoId,
            Tema = request.Tema,
            Cantidad = request.Cantidad
        };

        var created = await _repository.CreateAsync(capacitacionEventos);
        return _mapper.Map<CapacitacionEventosDto>(created);
    }
}

public class UpdateCapacitacionEventosHandler : IRequestHandler<UpdateCapacitacionEventosCommand, CapacitacionEventosDto>
{
    private readonly ICapacitacionEventosRepository _repository;
    private readonly IMapper _mapper;

    public UpdateCapacitacionEventosHandler(ICapacitacionEventosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CapacitacionEventosDto> Handle(UpdateCapacitacionEventosCommand request, CancellationToken cancellationToken)
    {
        var capacitacionEventos = await _repository.GetByIdAsync(request.CapacitacionEventosId);
        
        if (capacitacionEventos == null)
        {
            throw new KeyNotFoundException($"CapacitacionEventos with ID {request.CapacitacionEventosId} not found");
        }

        capacitacionEventos.Tema = request.Tema;
        capacitacionEventos.Cantidad = request.Cantidad;

        var updated = await _repository.UpdateAsync(capacitacionEventos);
        return _mapper.Map<CapacitacionEventosDto>(updated);
    }
}

public class DeleteCapacitacionEventosHandler : IRequestHandler<DeleteCapacitacionEventosCommand, bool>
{
    private readonly ICapacitacionEventosRepository _repository;

    public DeleteCapacitacionEventosHandler(ICapacitacionEventosRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteCapacitacionEventosCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.CapacitacionEventosId);
    }
}
