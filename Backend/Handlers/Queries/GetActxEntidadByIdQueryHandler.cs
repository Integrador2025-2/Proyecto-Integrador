using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.ActxEntidad;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetActxEntidadByIdQueryHandler : IRequestHandler<GetActxEntidadByIdQuery, ActxEntidadDto?>
{
    private readonly IActXEntidadRepository _repo;
    private readonly IMapper _mapper;

    public GetActxEntidadByIdQueryHandler(IActXEntidadRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ActxEntidadDto?> Handle(GetActxEntidadByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.Id);
        return item == null ? null : _mapper.Map<ActxEntidadDto>(item);
    }
}
