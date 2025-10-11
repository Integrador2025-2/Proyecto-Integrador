using AutoMapper;
using Backend.Commands.EquiposSoftware;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateEquiposSoftwareCommandHandler : IRequestHandler<CreateEquiposSoftwareCommand, EquiposSoftwareDto>
{
    private readonly IEquiposSoftwareRepository _repo;
    private readonly IMapper _mapper;
    public CreateEquiposSoftwareCommandHandler(IEquiposSoftwareRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<EquiposSoftwareDto> Handle(CreateEquiposSoftwareCommand request, CancellationToken cancellationToken)
    {
        var entity = new EquiposSoftware { RubroId = request.RubroId, EspecificacionesTecnicas = request.EspecificacionesTecnicas, Cantidad = request.Cantidad, Total = request.Total, RagEstado = request.RagEstado, PeriodoNum = request.PeriodoNum, PeriodoTipo = request.PeriodoTipo };
        var created = await _repo.CreateAsync(entity);
        return _mapper.Map<EquiposSoftwareDto>(created);
    }
}
