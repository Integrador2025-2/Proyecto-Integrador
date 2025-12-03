using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.ServiciosTecnologicos;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetAllServiciosTecnologicosQueryHandler : IRequestHandler<GetAllServiciosTecnologicosQuery, List<ServiciosTecnologicosDto>>
{
    private readonly IServiciosTecnologicosRepository _repo;
    private readonly IMapper _mapper;
    public GetAllServiciosTecnologicosQueryHandler(IServiciosTecnologicosRepository repo, IMapper mapper)
    {
        _repo = repo; _mapper = mapper;
    }

    public async Task<List<ServiciosTecnologicosDto>> Handle(GetAllServiciosTecnologicosQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetAllAsync();
        return _mapper.Map<List<ServiciosTecnologicosDto>>(items);
    }
}
