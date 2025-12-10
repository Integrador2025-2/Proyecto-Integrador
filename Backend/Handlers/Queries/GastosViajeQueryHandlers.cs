using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.GastosViaje;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetGastosViajeByIdHandler : IRequestHandler<GetGastosViajeByIdQuery, GastosViajeDto?>
{
    private readonly IGastosViajeRepository _repository;
    private readonly IMapper _mapper;

    public GetGastosViajeByIdHandler(IGastosViajeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GastosViajeDto?> Handle(GetGastosViajeByIdQuery request, CancellationToken cancellationToken)
    {
        var gastosViaje = await _repository.GetByIdAsync(request.GastosViajeId);
        return _mapper.Map<GastosViajeDto>(gastosViaje);
    }
}

public class GetAllGastosViajeHandler : IRequestHandler<GetAllGastosViajeQuery, IEnumerable<GastosViajeDto>>
{
    private readonly IGastosViajeRepository _repository;
    private readonly IMapper _mapper;

    public GetAllGastosViajeHandler(IGastosViajeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GastosViajeDto>> Handle(GetAllGastosViajeQuery request, CancellationToken cancellationToken)
    {
        var gastosViaje = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<GastosViajeDto>>(gastosViaje);
    }
}

public class GetGastosViajeByRecursoEspecificoIdHandler : IRequestHandler<GetGastosViajeByRecursoEspecificoIdQuery, GastosViajeDto?>
{
    private readonly IGastosViajeRepository _repository;
    private readonly IMapper _mapper;

    public GetGastosViajeByRecursoEspecificoIdHandler(IGastosViajeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GastosViajeDto?> Handle(GetGastosViajeByRecursoEspecificoIdQuery request, CancellationToken cancellationToken)
    {
        var gastosViaje = await _repository.GetByRecursoEspecificoIdAsync(request.RecursoEspecificoId);
        return _mapper.Map<GastosViajeDto>(gastosViaje);
    }
}
