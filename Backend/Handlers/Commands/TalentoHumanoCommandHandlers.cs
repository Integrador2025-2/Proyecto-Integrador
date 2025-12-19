using AutoMapper;
using Backend.Commands.TalentoHumano;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateTalentoHumanoHandler : IRequestHandler<CreateTalentoHumanoCommand, TalentoHumanoDto>
{
    private readonly ITalentoHumanoRepository _repository;
    private readonly IMapper _mapper;

    public CreateTalentoHumanoHandler(ITalentoHumanoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TalentoHumanoDto> Handle(CreateTalentoHumanoCommand request, CancellationToken cancellationToken)
    {
        var talentoHumano = new TalentoHumano
        {
            RecursoEspecificoId = request.RecursoEspecificoId,
            ContratacionId = request.ContratacionId,
            CargoEspecifico = request.CargoEspecifico,
            Semanas = request.Semanas,
            Total = request.Total
        };

        var created = await _repository.CreateAsync(talentoHumano);
        return _mapper.Map<TalentoHumanoDto>(created);
    }
}

public class UpdateTalentoHumanoHandler : IRequestHandler<UpdateTalentoHumanoCommand, TalentoHumanoDto>
{
    private readonly ITalentoHumanoRepository _repository;
    private readonly IMapper _mapper;

    public UpdateTalentoHumanoHandler(ITalentoHumanoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TalentoHumanoDto> Handle(UpdateTalentoHumanoCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.TalentoHumanoId);
        if (existing == null)
        {
            throw new KeyNotFoundException($"TalentoHumano with ID {request.TalentoHumanoId} not found.");
        }

        existing.ContratacionId = request.ContratacionId;
        existing.CargoEspecifico = request.CargoEspecifico;
        existing.Semanas = request.Semanas;
        existing.Total = request.Total;

        var updated = await _repository.UpdateAsync(existing);
        return _mapper.Map<TalentoHumanoDto>(updated);
    }
}

public class DeleteTalentoHumanoHandler : IRequestHandler<DeleteTalentoHumanoCommand>
{
    private readonly ITalentoHumanoRepository _repository;

    public DeleteTalentoHumanoHandler(ITalentoHumanoRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteTalentoHumanoCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.TalentoHumanoId);
    }
}
