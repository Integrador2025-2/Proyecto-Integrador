using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.CapacitacionEventos;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetCapacitacionEventosByIdHandler : IRequestHandler<GetCapacitacionEventosByIdQuery, CapacitacionEventosDto>
{
    private readonly ICapacitacionEventosRepository _repository;
    private readonly IMapper _mapper;

    public GetCapacitacionEventosByIdHandler(ICapacitacionEventosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CapacitacionEventosDto> Handle(GetCapacitacionEventosByIdQuery request, CancellationToken cancellationToken)
    {
        var capacitacionEventos = await _repository.GetByIdAsync(request.CapacitacionEventosId);
        return _mapper.Map<CapacitacionEventosDto>(capacitacionEventos);
    }
}

public class GetAllCapacitacionEventosHandler : IRequestHandler<GetAllCapacitacionEventosQuery, IEnumerable<CapacitacionEventosDto>>
{
    private readonly ICapacitacionEventosRepository _repository;
    private readonly IMapper _mapper;

    public GetAllCapacitacionEventosHandler(ICapacitacionEventosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CapacitacionEventosDto>> Handle(GetAllCapacitacionEventosQuery request, CancellationToken cancellationToken)
    {
        var capacitacionEventos = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<CapacitacionEventosDto>>(capacitacionEventos);
    }
}

public class GetCapacitacionEventosByRecursoEspecificoIdHandler : IRequestHandler<GetCapacitacionEventosByRecursoEspecificoIdQuery, CapacitacionEventosDto?>
{
    private readonly ICapacitacionEventosRepository _repository;
    private readonly IMapper _mapper;

    public GetCapacitacionEventosByRecursoEspecificoIdHandler(ICapacitacionEventosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CapacitacionEventosDto?> Handle(GetCapacitacionEventosByRecursoEspecificoIdQuery request, CancellationToken cancellationToken)
    {
        var capacitacionEventos = await _repository.GetByRecursoEspecificoIdAsync(request.RecursoEspecificoId);
        return _mapper.Map<CapacitacionEventosDto?>(capacitacionEventos);
    }
}
