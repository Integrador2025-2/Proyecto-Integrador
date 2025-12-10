using AutoMapper;
using Backend.Commands.RemuneracionPorAnio;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateRemuneracionPorAnioHandler : IRequestHandler<CreateRemuneracionPorAnioCommand, RemuneracionPorAnioDto>
{
    private readonly IRemuneracionPorAnioRepository _repository;
    private readonly IMapper _mapper;

    public CreateRemuneracionPorAnioHandler(IRemuneracionPorAnioRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RemuneracionPorAnioDto> Handle(CreateRemuneracionPorAnioCommand request, CancellationToken cancellationToken)
    {
        var remuneracionPorAnio = new RemuneracionPorAnio
        {
            TalentoHumanoId = request.TalentoHumanoId,
            Anio = request.Anio,
            Honorarios = request.Honorarios,
            ValorHora = request.ValorHora,
            SemanasAnio = request.SemanasAnio,
            TotalAnio = request.TotalAnio
        };

        var created = await _repository.CreateAsync(remuneracionPorAnio);
        return _mapper.Map<RemuneracionPorAnioDto>(created);
    }
}

public class UpdateRemuneracionPorAnioHandler : IRequestHandler<UpdateRemuneracionPorAnioCommand, RemuneracionPorAnioDto>
{
    private readonly IRemuneracionPorAnioRepository _repository;
    private readonly IMapper _mapper;

    public UpdateRemuneracionPorAnioHandler(IRemuneracionPorAnioRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RemuneracionPorAnioDto> Handle(UpdateRemuneracionPorAnioCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.RemuneracionPorAnioId);
        if (existing == null)
        {
            throw new KeyNotFoundException($"RemuneracionPorAnio with ID {request.RemuneracionPorAnioId} not found.");
        }

        existing.TalentoHumanoId = request.TalentoHumanoId;
        existing.Anio = request.Anio;
        existing.Honorarios = request.Honorarios;
        existing.ValorHora = request.ValorHora;
        existing.SemanasAnio = request.SemanasAnio;
        existing.TotalAnio = request.TotalAnio;

        var updated = await _repository.UpdateAsync(existing);
        return _mapper.Map<RemuneracionPorAnioDto>(updated);
    }
}

public class DeleteRemuneracionPorAnioHandler : IRequestHandler<DeleteRemuneracionPorAnioCommand>
{
    private readonly IRemuneracionPorAnioRepository _repository;

    public DeleteRemuneracionPorAnioHandler(IRemuneracionPorAnioRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteRemuneracionPorAnioCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.RemuneracionPorAnioId);
    }
}
