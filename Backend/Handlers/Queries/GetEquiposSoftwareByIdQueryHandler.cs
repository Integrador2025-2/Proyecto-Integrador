using AutoMapper;
using Backend.Queries.EquiposSoftware;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetEquiposSoftwareByIdQueryHandler : IRequestHandler<GetEquiposSoftwareByIdQuery, EquiposSoftwareDto?>
{
    private readonly IEquiposSoftwareRepository _repo;
    private readonly IMapper _mapper;
    public GetEquiposSoftwareByIdQueryHandler(IEquiposSoftwareRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<EquiposSoftwareDto?> Handle(GetEquiposSoftwareByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.Id);
        return item == null ? null : _mapper.Map<EquiposSoftwareDto>(item);
    }
}
