using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.EquiposSoftware;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetEquiposSoftwareByIdHandler : IRequestHandler<GetEquiposSoftwareByIdQuery, EquiposSoftwareDto>
{
    private readonly IEquiposSoftwareRepository _repository;
    private readonly IMapper _mapper;

    public GetEquiposSoftwareByIdHandler(IEquiposSoftwareRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EquiposSoftwareDto> Handle(GetEquiposSoftwareByIdQuery request, CancellationToken cancellationToken)
    {
        var equiposSoftware = await _repository.GetByIdAsync(request.EquiposSoftwareId);
        return _mapper.Map<EquiposSoftwareDto>(equiposSoftware);
    }
}

public class GetAllEquiposSoftwareHandler : IRequestHandler<GetAllEquiposSoftwareQuery, IEnumerable<EquiposSoftwareDto>>
{
    private readonly IEquiposSoftwareRepository _repository;
    private readonly IMapper _mapper;

    public GetAllEquiposSoftwareHandler(IEquiposSoftwareRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EquiposSoftwareDto>> Handle(GetAllEquiposSoftwareQuery request, CancellationToken cancellationToken)
    {
        var equiposSoftware = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<EquiposSoftwareDto>>(equiposSoftware);
    }
}

public class GetEquiposSoftwareByRecursoEspecificoIdHandler : IRequestHandler<GetEquiposSoftwareByRecursoEspecificoIdQuery, EquiposSoftwareDto?>
{
    private readonly IEquiposSoftwareRepository _repository;
    private readonly IMapper _mapper;

    public GetEquiposSoftwareByRecursoEspecificoIdHandler(IEquiposSoftwareRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EquiposSoftwareDto?> Handle(GetEquiposSoftwareByRecursoEspecificoIdQuery request, CancellationToken cancellationToken)
    {
        var equiposSoftware = await _repository.GetByRecursoEspecificoIdAsync(request.RecursoEspecificoId);
        return _mapper.Map<EquiposSoftwareDto?>(equiposSoftware);
    }
}
