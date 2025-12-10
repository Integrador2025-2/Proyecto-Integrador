using AutoMapper;
using Backend.Commands.ActxEntidad;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateActxEntidadCommandHandler : IRequestHandler<CreateActxEntidadCommand, ActxEntidadDto>
{
    private readonly IActXEntidadRepository _repo;
    private readonly IMapper _mapper;

    public CreateActxEntidadCommandHandler(IActXEntidadRepository repo, IMapper mapper)
    {
        _repo = repo; _mapper = mapper;
    }

    public async Task<ActxEntidadDto> Handle(CreateActxEntidadCommand request, CancellationToken cancellationToken)
    {
        var entity = new ActXEntidad
        {
            ActividadId = request.ActividadId,
            EntidadId = request.EntidadId,
            Efectivo = request.Efectivo,
            Especie = request.Especie
        };
        var created = await _repo.CreateAsync(entity);
        return _mapper.Map<ActxEntidadDto>(created);
    }
}
