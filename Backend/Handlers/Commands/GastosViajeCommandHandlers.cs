using AutoMapper;
using Backend.Commands.GastosViaje;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateGastosViajeHandler : IRequestHandler<CreateGastosViajeCommand, GastosViajeDto>
{
    private readonly IGastosViajeRepository _repository;
    private readonly IMapper _mapper;

    public CreateGastosViajeHandler(IGastosViajeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GastosViajeDto> Handle(CreateGastosViajeCommand request, CancellationToken cancellationToken)
    {
        var gastosViaje = new GastosViaje
        {
            RecursoEspecificoId = request.RecursoEspecificoId,
            Costo = request.Costo
        };

        var created = await _repository.CreateAsync(gastosViaje);
        return _mapper.Map<GastosViajeDto>(created);
    }
}

public class UpdateGastosViajeHandler : IRequestHandler<UpdateGastosViajeCommand, GastosViajeDto>
{
    private readonly IGastosViajeRepository _repository;
    private readonly IMapper _mapper;

    public UpdateGastosViajeHandler(IGastosViajeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GastosViajeDto> Handle(UpdateGastosViajeCommand request, CancellationToken cancellationToken)
    {
        var gastosViaje = await _repository.GetByIdAsync(request.GastosViajeId);
        if (gastosViaje == null)
        {
            throw new KeyNotFoundException($"GastosViaje con ID {request.GastosViajeId} no encontrado");
        }

        gastosViaje.Costo = request.Costo;

        var updated = await _repository.UpdateAsync(gastosViaje);
        return _mapper.Map<GastosViajeDto>(updated);
    }
}

public class DeleteGastosViajeHandler : IRequestHandler<DeleteGastosViajeCommand, bool>
{
    private readonly IGastosViajeRepository _repository;

    public DeleteGastosViajeHandler(IGastosViajeRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteGastosViajeCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsAsync(request.GastosViajeId);
        if (!exists)
        {
            throw new KeyNotFoundException($"GastosViaje con ID {request.GastosViajeId} no encontrado");
        }

        return await _repository.DeleteAsync(request.GastosViajeId);
    }
}
