using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Rubros;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetRubroByIdHandler : IRequestHandler<GetRubroByIdQuery, RubroDto?>
{
    private readonly IRubroRepository _repository;
    private readonly IMapper _mapper;

    public GetRubroByIdHandler(IRubroRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RubroDto?> Handle(GetRubroByIdQuery request, CancellationToken cancellationToken)
    {
        var rubro = await _repository.GetByIdAsync(request.RubroId);
        return _mapper.Map<RubroDto>(rubro);
    }
}

public class GetAllRubrosHandler : IRequestHandler<GetAllRubrosQuery, IEnumerable<RubroDto>>
{
    private readonly IRubroRepository _repository;
    private readonly IMapper _mapper;

    public GetAllRubrosHandler(IRubroRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RubroDto>> Handle(GetAllRubrosQuery request, CancellationToken cancellationToken)
    {
        var rubros = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<RubroDto>>(rubros);
    }
}
