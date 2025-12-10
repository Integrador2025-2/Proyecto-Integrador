using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.ServiciosTecnologicos;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetServiciosByRubroQueryHandler : IRequestHandler<GetServiciosByRubroQuery, List<ServiciosTecnologicosDto>>
{
    private readonly IServiciosTecnologicosRepository _repo;
    private readonly IMapper _mapper;
    public GetServiciosByRubroQueryHandler(IServiciosTecnologicosRepository repo, IMapper mapper)
    {
        _repo = repo; _mapper = mapper;
    }

    public async Task<List<ServiciosTecnologicosDto>> Handle(GetServiciosByRubroQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetByRubroIdAsync(request.RubroId);
        return _mapper.Map<List<ServiciosTecnologicosDto>>(items);
    }
}
