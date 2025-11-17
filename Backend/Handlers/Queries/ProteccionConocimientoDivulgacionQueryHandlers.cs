using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.ProteccionConocimientoDivulgacion;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetProteccionConocimientoDivulgacionByIdHandler : IRequestHandler<GetProteccionConocimientoDivulgacionByIdQuery, ProteccionConocimientoDivulgacionDto>
{
    private readonly IProteccionConocimientoDivulgacionRepository _repository;
    private readonly IMapper _mapper;

    public GetProteccionConocimientoDivulgacionByIdHandler(IProteccionConocimientoDivulgacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProteccionConocimientoDivulgacionDto> Handle(GetProteccionConocimientoDivulgacionByIdQuery request, CancellationToken cancellationToken)
    {
        var proteccionConocimientoDivulgacion = await _repository.GetByIdAsync(request.ProteccionId);
        return _mapper.Map<ProteccionConocimientoDivulgacionDto>(proteccionConocimientoDivulgacion);
    }
}

public class GetAllProteccionConocimientoDivulgacionHandler : IRequestHandler<GetAllProteccionConocimientoDivulgacionQuery, IEnumerable<ProteccionConocimientoDivulgacionDto>>
{
    private readonly IProteccionConocimientoDivulgacionRepository _repository;
    private readonly IMapper _mapper;

    public GetAllProteccionConocimientoDivulgacionHandler(IProteccionConocimientoDivulgacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProteccionConocimientoDivulgacionDto>> Handle(GetAllProteccionConocimientoDivulgacionQuery request, CancellationToken cancellationToken)
    {
        var proteccionConocimientoDivulgacion = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProteccionConocimientoDivulgacionDto>>(proteccionConocimientoDivulgacion);
    }
}

public class GetProteccionConocimientoDivulgacionByRecursoEspecificoIdHandler : IRequestHandler<GetProteccionConocimientoDivulgacionByRecursoEspecificoIdQuery, ProteccionConocimientoDivulgacionDto?>
{
    private readonly IProteccionConocimientoDivulgacionRepository _repository;
    private readonly IMapper _mapper;

    public GetProteccionConocimientoDivulgacionByRecursoEspecificoIdHandler(IProteccionConocimientoDivulgacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProteccionConocimientoDivulgacionDto?> Handle(GetProteccionConocimientoDivulgacionByRecursoEspecificoIdQuery request, CancellationToken cancellationToken)
    {
        var proteccionConocimientoDivulgacion = await _repository.GetByRecursoEspecificoIdAsync(request.RecursoEspecificoId);
        return _mapper.Map<ProteccionConocimientoDivulgacionDto?>(proteccionConocimientoDivulgacion);
    }
}
