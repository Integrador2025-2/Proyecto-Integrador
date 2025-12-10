using AutoMapper;
using Backend.Commands.Entidades;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateEntidadCommandHandler : IRequestHandler<CreateEntidadCommand, EntidadDto>
{
    private readonly IEntidadRepository _repo;
    private readonly IMapper _mapper;

    public CreateEntidadCommandHandler(IEntidadRepository repo, IMapper mapper)
    {
        _repo = repo; _mapper = mapper;
    }

    public async Task<EntidadDto> Handle(CreateEntidadCommand request, CancellationToken cancellationToken)
    {
        var entidad = new Entidad { Nombre = request.Nombre };
        var created = await _repo.CreateAsync(entidad);
        return _mapper.Map<EntidadDto>(created);
    }
}
