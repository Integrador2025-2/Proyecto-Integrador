using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Entidades;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetEntidadByIdQueryHandler : IRequestHandler<GetEntidadByIdQuery, EntidadDto?>
{
    private readonly IEntidadRepository _entidadRepository;
    private readonly IMapper _mapper;

    public GetEntidadByIdQueryHandler(IEntidadRepository entidadRepository, IMapper mapper)
    {
        _entidadRepository = entidadRepository;
        _mapper = mapper;
    }

    public async Task<EntidadDto?> Handle(GetEntidadByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _entidadRepository.GetByIdAsync(request.Id);
        return item == null ? null : _mapper.Map<EntidadDto>(item);
    }
}
