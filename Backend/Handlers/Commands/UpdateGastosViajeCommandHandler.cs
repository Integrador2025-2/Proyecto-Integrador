using AutoMapper;
using Backend.Commands.GastosViaje;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class UpdateGastosViajeCommandHandler : IRequestHandler<UpdateGastosViajeCommand, GastosViajeDto?>
{
    private readonly IGastosViajeRepository _repo;
    private readonly IMapper _mapper;
    public UpdateGastosViajeCommandHandler(IGastosViajeRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<GastosViajeDto?> Handle(UpdateGastosViajeCommand request, CancellationToken cancellationToken)
    {
        var entity = new GastosViaje { GastosViajeId = request.GastosViajeId, RubroId = request.RubroId, Costo = request.Costo, RagEstado = request.RagEstado, PeriodoNum = request.PeriodoNum, PeriodoTipo = request.PeriodoTipo };
        var updated = await _repo.UpdateAsync(entity);
        return updated == null ? null : _mapper.Map<GastosViajeDto>(updated);
    }
}
