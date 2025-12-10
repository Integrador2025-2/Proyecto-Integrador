using AutoMapper;
using Backend.Commands.GastosViaje;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateGastosViajeCommandHandler : IRequestHandler<CreateGastosViajeCommand, GastosViajeDto>
{
    private readonly IGastosViajeRepository _repo;
    private readonly IMapper _mapper;
    public CreateGastosViajeCommandHandler(IGastosViajeRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<GastosViajeDto> Handle(CreateGastosViajeCommand request, CancellationToken cancellationToken)
    {
        var entity = new GastosViaje { RubroId = request.RubroId, Costo = request.Costo, RagEstado = request.RagEstado, PeriodoNum = request.PeriodoNum, PeriodoTipo = request.PeriodoTipo };
        var created = await _repo.CreateAsync(entity);
        return _mapper.Map<GastosViajeDto>(created);
    }
}
