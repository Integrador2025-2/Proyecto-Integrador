using AutoMapper;
using Backend.Commands.TalentoHumano;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateTalentoHumanoCommandHandler : IRequestHandler<CreateTalentoHumanoCommand, TalentoHumanoDto>
{
    private readonly ITalentoHumanoRepository _repo;
    private readonly IMapper _mapper;
    public CreateTalentoHumanoCommandHandler(ITalentoHumanoRepository repo, IMapper mapper)
    {
        _repo = repo; _mapper = mapper;
    }

    public async Task<TalentoHumanoDto> Handle(CreateTalentoHumanoCommand request, CancellationToken cancellationToken)
    {
        var entity = new TalentoHumano
        {
            RubroId = request.RubroId,
            CargoEspecifico = request.CargoEspecifico,
            Semanas = request.Semanas,
            Total = request.Total,
            RagEstado = request.RagEstado,
            PeriodoNum = request.PeriodoNum,
            PeriodoTipo = request.PeriodoTipo
        };

        var created = await _repo.CreateAsync(entity);
        return _mapper.Map<TalentoHumanoDto>(created);
    }
}
