using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.ServiciosTecnologicos;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetServiciosTecnologicosByIdHandler : IRequestHandler<GetServiciosTecnologicosByIdQuery, ServiciosTecnologicosDto>
{
    private readonly IServiciosTecnologicosRepository _repository;
    private readonly IMapper _mapper;

    public GetServiciosTecnologicosByIdHandler(IServiciosTecnologicosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ServiciosTecnologicosDto> Handle(GetServiciosTecnologicosByIdQuery request, CancellationToken cancellationToken)
    {
        var serviciosTecnologicos = await _repository.GetByIdAsync(request.ServiciosTecnologicosId);
        return _mapper.Map<ServiciosTecnologicosDto>(serviciosTecnologicos);
    }
}

public class GetAllServiciosTecnologicosHandler : IRequestHandler<GetAllServiciosTecnologicosQuery, IEnumerable<ServiciosTecnologicosDto>>
{
    private readonly IServiciosTecnologicosRepository _repository;
    private readonly IMapper _mapper;

    public GetAllServiciosTecnologicosHandler(IServiciosTecnologicosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ServiciosTecnologicosDto>> Handle(GetAllServiciosTecnologicosQuery request, CancellationToken cancellationToken)
    {
        var serviciosTecnologicos = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ServiciosTecnologicosDto>>(serviciosTecnologicos);
    }
}

public class GetServiciosTecnologicosByRecursoEspecificoIdHandler : IRequestHandler<GetServiciosTecnologicosByRecursoEspecificoIdQuery, ServiciosTecnologicosDto?>
{
    private readonly IServiciosTecnologicosRepository _repository;
    private readonly IMapper _mapper;

    public GetServiciosTecnologicosByRecursoEspecificoIdHandler(IServiciosTecnologicosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ServiciosTecnologicosDto?> Handle(GetServiciosTecnologicosByRecursoEspecificoIdQuery request, CancellationToken cancellationToken)
    {
        var serviciosTecnologicos = await _repository.GetByRecursoEspecificoIdAsync(request.RecursoEspecificoId);
        return _mapper.Map<ServiciosTecnologicosDto?>(serviciosTecnologicos);
    }
}
