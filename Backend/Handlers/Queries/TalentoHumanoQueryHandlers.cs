using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.TalentoHumano;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetTalentoHumanoByIdHandler : IRequestHandler<GetTalentoHumanoByIdQuery, TalentoHumanoDto>
{
    private readonly ITalentoHumanoRepository _repository;
    private readonly IMapper _mapper;

    public GetTalentoHumanoByIdHandler(ITalentoHumanoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TalentoHumanoDto> Handle(GetTalentoHumanoByIdQuery request, CancellationToken cancellationToken)
    {
        var talentoHumano = await _repository.GetByIdAsync(request.TalentoHumanoId);
        return _mapper.Map<TalentoHumanoDto>(talentoHumano);
    }
}

public class GetAllTalentoHumanoHandler : IRequestHandler<GetAllTalentoHumanoQuery, IEnumerable<TalentoHumanoDto>>
{
    private readonly ITalentoHumanoRepository _repository;
    private readonly IMapper _mapper;

    public GetAllTalentoHumanoHandler(ITalentoHumanoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TalentoHumanoDto>> Handle(GetAllTalentoHumanoQuery request, CancellationToken cancellationToken)
    {
        var talentoHumano = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<TalentoHumanoDto>>(talentoHumano);
    }
}

public class GetTalentoHumanoByRecursoEspecificoIdHandler : IRequestHandler<GetTalentoHumanoByRecursoEspecificoIdQuery, IEnumerable<TalentoHumanoDto>>
{
    private readonly ITalentoHumanoRepository _repository;
    private readonly IMapper _mapper;

    public GetTalentoHumanoByRecursoEspecificoIdHandler(ITalentoHumanoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TalentoHumanoDto>> Handle(GetTalentoHumanoByRecursoEspecificoIdQuery request, CancellationToken cancellationToken)
    {
        var talentoHumano = await _repository.GetByRecursoEspecificoIdAsync(request.RecursoEspecificoId);
        return _mapper.Map<IEnumerable<TalentoHumanoDto>>(talentoHumano);
    }
}
