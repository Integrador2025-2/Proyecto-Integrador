using AutoMapper;
using Backend.Commands.ServiciosTecnologicos;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class UpdateServiciosTecnologicosCommandHandler : IRequestHandler<UpdateServiciosTecnologicosCommand, ServiciosTecnologicosDto?>
{
    private readonly IServiciosTecnologicosRepository _repo;
    private readonly IMapper _mapper;
    public UpdateServiciosTecnologicosCommandHandler(IServiciosTecnologicosRepository repo, IMapper mapper)
    {
        _repo = repo; _mapper = mapper;
    }

    public async Task<ServiciosTecnologicosDto?> Handle(UpdateServiciosTecnologicosCommand request, CancellationToken cancellationToken)
    {
        var entity = new ServiciosTecnologicos
        {
            ServiciosTecnologicosId = request.ServiciosTecnologicosId,
            RubroId = request.RubroId,
            Descripcion = request.Descripcion,
            Total = request.Total,
            RagEstado = request.RagEstado,
            PeriodoNum = request.PeriodoNum,
            PeriodoTipo = request.PeriodoTipo
        };

        var updated = await _repo.UpdateAsync(entity);
        return updated == null ? null : _mapper.Map<ServiciosTecnologicosDto>(updated);
    }
}
