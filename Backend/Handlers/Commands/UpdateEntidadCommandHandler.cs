using AutoMapper;
using Backend.Commands.Entidades;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class UpdateEntidadCommandHandler : IRequestHandler<UpdateEntidadCommand, EntidadDto>
{
    private readonly IEntidadRepository _repo;
    private readonly IMapper _mapper;

    public UpdateEntidadCommandHandler(IEntidadRepository repo, IMapper mapper)
    {
        _repo = repo; _mapper = mapper;
    }

    public async Task<EntidadDto> Handle(UpdateEntidadCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repo.GetByIdAsync(request.EntidadId);
        if (existing == null) throw new KeyNotFoundException("Entidad not found");
        existing.Nombre = request.Nombre;
        var updated = await _repo.UpdateAsync(existing);
        return _mapper.Map<EntidadDto>(updated);
    }
}
