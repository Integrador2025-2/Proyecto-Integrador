using AutoMapper;
using Backend.Commands.TalentoHumano;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class UpdateTalentoHumanoCommandHandler : IRequestHandler<UpdateTalentoHumanoCommand, TalentoHumanoDto?>
{
    private readonly ITalentoHumanoRepository _repo;
    private readonly IMapper _mapper;
    public UpdateTalentoHumanoCommandHandler(ITalentoHumanoRepository repo, IMapper mapper)
    {
        _repo = repo; _mapper = mapper;
    }

    public async Task<TalentoHumanoDto?> Handle(UpdateTalentoHumanoCommand request, CancellationToken cancellationToken)
    {
        var entity = new TalentoHumano
        {
            TalentoHumanoId = request.TalentoHumanoId,
            RubroId = request.RubroId,
            CargoEspecifico = request.CargoEspecifico,
            Semanas = request.Semanas,
            Total = request.Total,
            RagEstado = request.RagEstado,
            PeriodoNum = request.PeriodoNum,
            PeriodoTipo = request.PeriodoTipo
        };

        var updated = await _repo.UpdateAsync(entity);
        return updated == null ? null : _mapper.Map<TalentoHumanoDto>(updated);
    }
}
