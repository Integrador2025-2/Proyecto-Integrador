using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Entidades;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetAllEntidadesQueryHandler : IRequestHandler<GetAllEntidadesQuery, List<EntidadDto>>
{
    private readonly IEntidadRepository _entidadRepository;
    private readonly IMapper _mapper;

    public GetAllEntidadesQueryHandler(IEntidadRepository entidadRepository, IMapper mapper)
    {
        _entidadRepository = entidadRepository;
        _mapper = mapper;
    }

    public async Task<List<EntidadDto>> Handle(GetAllEntidadesQuery request, CancellationToken cancellationToken)
    {
        var items = await _entidadRepository.GetByFilterAsync(request.SearchTerm);
        return _mapper.Map<List<EntidadDto>>(items);
    }
}
