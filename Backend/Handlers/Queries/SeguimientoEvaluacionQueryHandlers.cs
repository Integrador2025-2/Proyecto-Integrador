using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.SeguimientoEvaluacion;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetSeguimientoEvaluacionByIdHandler : IRequestHandler<GetSeguimientoEvaluacionByIdQuery, SeguimientoEvaluacionDto>
{
    private readonly ISeguimientoEvaluacionRepository _repository;
    private readonly IMapper _mapper;

    public GetSeguimientoEvaluacionByIdHandler(ISeguimientoEvaluacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SeguimientoEvaluacionDto> Handle(GetSeguimientoEvaluacionByIdQuery request, CancellationToken cancellationToken)
    {
        var seguimientoEvaluacion = await _repository.GetByIdAsync(request.SeguimientoId);
        return _mapper.Map<SeguimientoEvaluacionDto>(seguimientoEvaluacion);
    }
}

public class GetAllSeguimientoEvaluacionHandler : IRequestHandler<GetAllSeguimientoEvaluacionQuery, IEnumerable<SeguimientoEvaluacionDto>>
{
    private readonly ISeguimientoEvaluacionRepository _repository;
    private readonly IMapper _mapper;

    public GetAllSeguimientoEvaluacionHandler(ISeguimientoEvaluacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SeguimientoEvaluacionDto>> Handle(GetAllSeguimientoEvaluacionQuery request, CancellationToken cancellationToken)
    {
        var seguimientoEvaluacion = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<SeguimientoEvaluacionDto>>(seguimientoEvaluacion);
    }
}

public class GetSeguimientoEvaluacionByRecursoEspecificoIdHandler : IRequestHandler<GetSeguimientoEvaluacionByRecursoEspecificoIdQuery, SeguimientoEvaluacionDto?>
{
    private readonly ISeguimientoEvaluacionRepository _repository;
    private readonly IMapper _mapper;

    public GetSeguimientoEvaluacionByRecursoEspecificoIdHandler(ISeguimientoEvaluacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SeguimientoEvaluacionDto?> Handle(GetSeguimientoEvaluacionByRecursoEspecificoIdQuery request, CancellationToken cancellationToken)
    {
        var seguimientoEvaluacion = await _repository.GetByRecursoEspecificoIdAsync(request.RecursoEspecificoId);
        return _mapper.Map<SeguimientoEvaluacionDto?>(seguimientoEvaluacion);
    }
}
