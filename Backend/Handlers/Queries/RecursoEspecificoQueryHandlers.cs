using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.RecursosEspecificos;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetRecursoEspecificoByIdHandler : IRequestHandler<GetRecursoEspecificoByIdQuery, RecursoEspecificoDto?>
{
    private readonly IRecursoEspecificoRepository _repository;
    private readonly IMapper _mapper;

    public GetRecursoEspecificoByIdHandler(IRecursoEspecificoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RecursoEspecificoDto?> Handle(GetRecursoEspecificoByIdQuery request, CancellationToken cancellationToken)
    {
        var recursoEspecifico = await _repository.GetByIdAsync(request.RecursoEspecificoId);
        return _mapper.Map<RecursoEspecificoDto>(recursoEspecifico);
    }
}

public class GetAllRecursosEspecificosHandler : IRequestHandler<GetAllRecursosEspecificosQuery, IEnumerable<RecursoEspecificoDto>>
{
    private readonly IRecursoEspecificoRepository _repository;
    private readonly IMapper _mapper;

    public GetAllRecursosEspecificosHandler(IRecursoEspecificoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RecursoEspecificoDto>> Handle(GetAllRecursosEspecificosQuery request, CancellationToken cancellationToken)
    {
        var recursosEspecificos = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<RecursoEspecificoDto>>(recursosEspecificos);
    }
}

public class GetRecursosEspecificosByRecursoIdHandler : IRequestHandler<GetRecursosEspecificosByRecursoIdQuery, IEnumerable<RecursoEspecificoDto>>
{
    private readonly IRecursoEspecificoRepository _repository;
    private readonly IMapper _mapper;

    public GetRecursosEspecificosByRecursoIdHandler(IRecursoEspecificoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RecursoEspecificoDto>> Handle(GetRecursosEspecificosByRecursoIdQuery request, CancellationToken cancellationToken)
    {
        var recursosEspecificos = await _repository.GetByRecursoIdAsync(request.RecursoId);
        return _mapper.Map<IEnumerable<RecursoEspecificoDto>>(recursosEspecificos);
    }
}

public class GetRecursosEspecificosByTipoHandler : IRequestHandler<GetRecursosEspecificosByTipoQuery, IEnumerable<RecursoEspecificoDto>>
{
    private readonly IRecursoEspecificoRepository _repository;
    private readonly IMapper _mapper;

    public GetRecursosEspecificosByTipoHandler(IRecursoEspecificoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RecursoEspecificoDto>> Handle(GetRecursosEspecificosByTipoQuery request, CancellationToken cancellationToken)
    {
        var recursosEspecificos = await _repository.GetByTipoAsync(request.Tipo);
        return _mapper.Map<IEnumerable<RecursoEspecificoDto>>(recursosEspecificos);
    }
}
