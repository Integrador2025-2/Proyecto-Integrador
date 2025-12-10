using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Rubros;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetAllRubrosQueryHandler : IRequestHandler<GetAllRubrosQuery, List<RubroDto>>
{
    private readonly IRubroRepository _rubroRepository;
    private readonly IMapper _mapper;
    public GetAllRubrosQueryHandler(IRubroRepository rubroRepository, IMapper mapper)
    {
        _rubroRepository = rubroRepository; _mapper = mapper;
    }

    public async Task<List<RubroDto>> Handle(GetAllRubrosQuery request, CancellationToken cancellationToken)
    {
        var items = await _rubroRepository.GetAllAsync();
        return _mapper.Map<List<RubroDto>>(items);
    }
}
