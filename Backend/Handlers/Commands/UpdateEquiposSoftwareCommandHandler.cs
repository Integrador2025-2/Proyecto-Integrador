using AutoMapper;
using Backend.Commands.EquiposSoftware;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class UpdateEquiposSoftwareCommandHandler : IRequestHandler<UpdateEquiposSoftwareCommand, EquiposSoftwareDto?>
{
    private readonly IEquiposSoftwareRepository _repo;
    private readonly IMapper _mapper;
    public UpdateEquiposSoftwareCommandHandler(IEquiposSoftwareRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<EquiposSoftwareDto?> Handle(UpdateEquiposSoftwareCommand request, CancellationToken cancellationToken)
    {
        var entity = new EquiposSoftware { EquiposSoftwareId = request.EquiposSoftwareId, RubroId = request.RubroId, EspecificacionesTecnicas = request.EspecificacionesTecnicas, Cantidad = request.Cantidad, Total = request.Total, RagEstado = request.RagEstado, PeriodoNum = request.PeriodoNum, PeriodoTipo = request.PeriodoTipo };
        var updated = await _repo.UpdateAsync(entity);
        return updated == null ? null : _mapper.Map<EquiposSoftwareDto>(updated);
    }
}
