using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.ServiciosTecnologicos;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetServiciosTecnologicosByIdQueryHandler : IRequestHandler<GetServiciosTecnologicosByIdQuery, ServiciosTecnologicosDto?>
{
    private readonly IServiciosTecnologicosRepository _repo;
    private readonly IMapper _mapper;
    public GetServiciosTecnologicosByIdQueryHandler(IServiciosTecnologicosRepository repo, IMapper mapper)
    {
        _repo = repo; _mapper = mapper;
    }

    public async Task<ServiciosTecnologicosDto?> Handle(GetServiciosTecnologicosByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.ServiciosTecnologicosId);
        return item == null ? null : _mapper.Map<ServiciosTecnologicosDto>(item);
    }
}
