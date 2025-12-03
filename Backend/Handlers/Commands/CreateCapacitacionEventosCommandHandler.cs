using AutoMapper;
using Backend.Commands.CapacitacionEventos;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateCapacitacionEventosCommandHandler : IRequestHandler<CreateCapacitacionEventosCommand, CapacitacionEventosDto>
{
    private readonly ICapacitacionEventosRepository _repo;
    private readonly IMapper _mapper;
    public CreateCapacitacionEventosCommandHandler(ICapacitacionEventosRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<CapacitacionEventosDto> Handle(CreateCapacitacionEventosCommand request, CancellationToken cancellationToken)
    {
        var entity = new CapacitacionEventos 
        { 
            RubroId = request.RubroId, 
            Tema = request.Tema, 
            Cantidad = request.Cantidad, 
            Total = request.Total, 
            RagEstado = request.RagEstado, 
            PeriodoNum = request.PeriodoNum, 
            PeriodoTipo = request.PeriodoTipo,
            ActividadId = request.ActividadId
        };
        var created = await _repo.CreateAsync(entity);
        return _mapper.Map<CapacitacionEventosDto>(created);
    }
}

