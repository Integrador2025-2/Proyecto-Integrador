using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Contratacion;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetContratacionByIdHandler : IRequestHandler<GetContratacionByIdQuery, ContratacionDto>
{
    private readonly IContratacionRepository _repository;
    private readonly IMapper _mapper;

    public GetContratacionByIdHandler(IContratacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ContratacionDto> Handle(GetContratacionByIdQuery request, CancellationToken cancellationToken)
    {
        var contratacion = await _repository.GetByIdAsync(request.ContratacionId);
        return _mapper.Map<ContratacionDto>(contratacion);
    }
}

public class GetAllContratacionesHandler : IRequestHandler<GetAllContratacionesQuery, IEnumerable<ContratacionDto>>
{
    private readonly IContratacionRepository _repository;
    private readonly IMapper _mapper;

    public GetAllContratacionesHandler(IContratacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ContratacionDto>> Handle(GetAllContratacionesQuery request, CancellationToken cancellationToken)
    {
        var contrataciones = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ContratacionDto>>(contrataciones);
    }
}

public class GetContratacionesByCategoriaHandler : IRequestHandler<GetContratacionesByCategoriaQuery, IEnumerable<ContratacionDto>>
{
    private readonly IContratacionRepository _repository;
    private readonly IMapper _mapper;

    public GetContratacionesByCategoriaHandler(IContratacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ContratacionDto>> Handle(GetContratacionesByCategoriaQuery request, CancellationToken cancellationToken)
    {
        var contrataciones = await _repository.GetByCategoriaAsync(request.Categoria);
        return _mapper.Map<IEnumerable<ContratacionDto>>(contrataciones);
    }
}
