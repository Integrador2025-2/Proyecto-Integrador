using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.TalentoHumanoTareas;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetTalentoHumanoTareaByIdHandler : IRequestHandler<GetTalentoHumanoTareaByIdQuery, TalentoHumanoTareaDto>
{
    private readonly ITalentoHumanoTareaRepository _repository;
    private readonly IMapper _mapper;

    public GetTalentoHumanoTareaByIdHandler(ITalentoHumanoTareaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TalentoHumanoTareaDto> Handle(GetTalentoHumanoTareaByIdQuery request, CancellationToken cancellationToken)
    {
        var talentoHumanoTarea = await _repository.GetByIdAsync(request.TalentoHumanoTareasId);
        return _mapper.Map<TalentoHumanoTareaDto>(talentoHumanoTarea);
    }
}

public class GetAllTalentoHumanoTareasHandler : IRequestHandler<GetAllTalentoHumanoTareasQuery, IEnumerable<TalentoHumanoTareaDto>>
{
    private readonly ITalentoHumanoTareaRepository _repository;
    private readonly IMapper _mapper;

    public GetAllTalentoHumanoTareasHandler(ITalentoHumanoTareaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TalentoHumanoTareaDto>> Handle(GetAllTalentoHumanoTareasQuery request, CancellationToken cancellationToken)
    {
        var talentoHumanoTareas = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<TalentoHumanoTareaDto>>(talentoHumanoTareas);
    }
}

public class GetTalentoHumanoTareasByTalentoHumanoIdHandler : IRequestHandler<GetTalentoHumanoTareasByTalentoHumanoIdQuery, IEnumerable<TalentoHumanoTareaDto>>
{
    private readonly ITalentoHumanoTareaRepository _repository;
    private readonly IMapper _mapper;

    public GetTalentoHumanoTareasByTalentoHumanoIdHandler(ITalentoHumanoTareaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TalentoHumanoTareaDto>> Handle(GetTalentoHumanoTareasByTalentoHumanoIdQuery request, CancellationToken cancellationToken)
    {
        var talentoHumanoTareas = await _repository.GetByTalentoHumanoIdAsync(request.TalentoHumanoId);
        return _mapper.Map<IEnumerable<TalentoHumanoTareaDto>>(talentoHumanoTareas);
    }
}

public class GetTalentoHumanoTareasByTareaIdHandler : IRequestHandler<GetTalentoHumanoTareasByTareaIdQuery, IEnumerable<TalentoHumanoTareaDto>>
{
    private readonly ITalentoHumanoTareaRepository _repository;
    private readonly IMapper _mapper;

    public GetTalentoHumanoTareasByTareaIdHandler(ITalentoHumanoTareaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TalentoHumanoTareaDto>> Handle(GetTalentoHumanoTareasByTareaIdQuery request, CancellationToken cancellationToken)
    {
        var talentoHumanoTareas = await _repository.GetByTareaIdAsync(request.TareaId);
        return _mapper.Map<IEnumerable<TalentoHumanoTareaDto>>(talentoHumanoTareas);
    }
}
