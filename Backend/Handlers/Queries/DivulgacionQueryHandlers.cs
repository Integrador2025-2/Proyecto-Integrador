using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Divulgacion;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetDivulgacionByIdHandler : IRequestHandler<GetDivulgacionByIdQuery, DivulgacionDto>
{
    private readonly IDivulgacionRepository _repository;
    private readonly IMapper _mapper;

    public GetDivulgacionByIdHandler(IDivulgacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DivulgacionDto> Handle(GetDivulgacionByIdQuery request, CancellationToken cancellationToken)
    {
        var divulgacion = await _repository.GetByIdAsync(request.DivulgacionId);
        return _mapper.Map<DivulgacionDto>(divulgacion);
    }
}

public class GetAllDivulgacionHandler : IRequestHandler<GetAllDivulgacionQuery, IEnumerable<DivulgacionDto>>
{
    private readonly IDivulgacionRepository _repository;
    private readonly IMapper _mapper;

    public GetAllDivulgacionHandler(IDivulgacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DivulgacionDto>> Handle(GetAllDivulgacionQuery request, CancellationToken cancellationToken)
    {
        var divulgacion = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<DivulgacionDto>>(divulgacion);
    }
}

public class GetDivulgacionByRecursoEspecificoIdHandler : IRequestHandler<GetDivulgacionByRecursoEspecificoIdQuery, DivulgacionDto?>
{
    private readonly IDivulgacionRepository _repository;
    private readonly IMapper _mapper;

    public GetDivulgacionByRecursoEspecificoIdHandler(IDivulgacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DivulgacionDto?> Handle(GetDivulgacionByRecursoEspecificoIdQuery request, CancellationToken cancellationToken)
    {
        var divulgacion = await _repository.GetByRecursoEspecificoIdAsync(request.RecursoEspecificoId);
        return _mapper.Map<DivulgacionDto?>(divulgacion);
    }
}
