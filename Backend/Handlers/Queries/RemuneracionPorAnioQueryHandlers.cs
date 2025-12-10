using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.RemuneracionPorAnio;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetRemuneracionPorAnioByIdHandler : IRequestHandler<GetRemuneracionPorAnioByIdQuery, RemuneracionPorAnioDto>
{
    private readonly IRemuneracionPorAnioRepository _repository;
    private readonly IMapper _mapper;

    public GetRemuneracionPorAnioByIdHandler(IRemuneracionPorAnioRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RemuneracionPorAnioDto> Handle(GetRemuneracionPorAnioByIdQuery request, CancellationToken cancellationToken)
    {
        var remuneracionPorAnio = await _repository.GetByIdAsync(request.RemuneracionPorAnioId);
        return _mapper.Map<RemuneracionPorAnioDto>(remuneracionPorAnio);
    }
}

public class GetAllRemuneracionPorAnioHandler : IRequestHandler<GetAllRemuneracionPorAnioQuery, IEnumerable<RemuneracionPorAnioDto>>
{
    private readonly IRemuneracionPorAnioRepository _repository;
    private readonly IMapper _mapper;

    public GetAllRemuneracionPorAnioHandler(IRemuneracionPorAnioRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RemuneracionPorAnioDto>> Handle(GetAllRemuneracionPorAnioQuery request, CancellationToken cancellationToken)
    {
        var remuneraciones = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<RemuneracionPorAnioDto>>(remuneraciones);
    }
}

public class GetRemuneracionPorAnioByTalentoHumanoIdHandler : IRequestHandler<GetRemuneracionPorAnioByTalentoHumanoIdQuery, IEnumerable<RemuneracionPorAnioDto>>
{
    private readonly IRemuneracionPorAnioRepository _repository;
    private readonly IMapper _mapper;

    public GetRemuneracionPorAnioByTalentoHumanoIdHandler(IRemuneracionPorAnioRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RemuneracionPorAnioDto>> Handle(GetRemuneracionPorAnioByTalentoHumanoIdQuery request, CancellationToken cancellationToken)
    {
        var remuneraciones = await _repository.GetByTalentoHumanoIdAsync(request.TalentoHumanoId);
        return _mapper.Map<IEnumerable<RemuneracionPorAnioDto>>(remuneraciones);
    }
}

public class GetRemuneracionPorAnioByAnioHandler : IRequestHandler<GetRemuneracionPorAnioByAnioQuery, IEnumerable<RemuneracionPorAnioDto>>
{
    private readonly IRemuneracionPorAnioRepository _repository;
    private readonly IMapper _mapper;

    public GetRemuneracionPorAnioByAnioHandler(IRemuneracionPorAnioRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RemuneracionPorAnioDto>> Handle(GetRemuneracionPorAnioByAnioQuery request, CancellationToken cancellationToken)
    {
        var remuneraciones = await _repository.GetByAnioAsync(request.Anio);
        return _mapper.Map<IEnumerable<RemuneracionPorAnioDto>>(remuneraciones);
    }
}
