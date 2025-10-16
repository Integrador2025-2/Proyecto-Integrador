using AutoMapper;
using Backend.Commands.ActxEntidad;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class UpdateActxEntidadCommandHandler : IRequestHandler<UpdateActxEntidadCommand, ActxEntidadDto>
{
    private readonly IActXEntidadRepository _repo;
    private readonly IMapper _mapper;

    public UpdateActxEntidadCommandHandler(IActXEntidadRepository repo, IMapper mapper)
    {
        _repo = repo; _mapper = mapper;
    }

    public async Task<ActxEntidadDto> Handle(UpdateActxEntidadCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repo.GetByIdAsync(request.ActXEntidadId);
        if (existing == null) throw new KeyNotFoundException("ActXEntidad not found");

        existing.ActividadId = request.ActividadId;
        existing.EntidadId = request.EntidadId;
        existing.Efectivo = request.Efectivo;
        existing.Especie = request.Especie;

        var updated = await _repo.UpdateAsync(existing);
        return _mapper.Map<ActxEntidadDto>(updated);
    }
}
