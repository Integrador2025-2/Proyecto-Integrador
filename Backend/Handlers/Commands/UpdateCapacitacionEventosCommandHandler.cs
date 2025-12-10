using AutoMapper;
using Backend.Commands.CapacitacionEventos;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class UpdateCapacitacionEventosCommandHandler : IRequestHandler<UpdateCapacitacionEventosCommand, CapacitacionEventosDto?>
{
    private readonly ICapacitacionEventosRepository _repo;
    private readonly IMapper _mapper;
    public UpdateCapacitacionEventosCommandHandler(ICapacitacionEventosRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<CapacitacionEventosDto?> Handle(UpdateCapacitacionEventosCommand request, CancellationToken cancellationToken)
    {
        var entity = new CapacitacionEventos { CapacitacionEventosId = request.CapacitacionEventosId, RubroId = request.RubroId, Tema = request.Tema, Cantidad = request.Cantidad, Total = request.Total, RagEstado = request.RagEstado, PeriodoNum = request.PeriodoNum, PeriodoTipo = request.PeriodoTipo };
        var updated = await _repo.UpdateAsync(entity);
        return updated == null ? null : _mapper.Map<CapacitacionEventosDto>(updated);
    }
}
