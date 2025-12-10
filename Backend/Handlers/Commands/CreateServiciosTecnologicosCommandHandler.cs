using AutoMapper;
using Backend.Commands.ServiciosTecnologicos;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateServiciosTecnologicosCommandHandler : IRequestHandler<CreateServiciosTecnologicosCommand, ServiciosTecnologicosDto>
{
    private readonly IServiciosTecnologicosRepository _repo;
    private readonly IMapper _mapper;
    public CreateServiciosTecnologicosCommandHandler(IServiciosTecnologicosRepository repo, IMapper mapper)
    {
        _repo = repo; _mapper = mapper;
    }

    public async Task<ServiciosTecnologicosDto> Handle(CreateServiciosTecnologicosCommand request, CancellationToken cancellationToken)
    {
        var entity = new ServiciosTecnologicos
        {
            RubroId = request.RubroId,
            Descripcion = request.Descripcion,
            Total = request.Total,
            RagEstado = request.RagEstado,
            PeriodoNum = request.PeriodoNum,
            PeriodoTipo = request.PeriodoTipo
        };

        var created = await _repo.CreateAsync(entity);
        return _mapper.Map<ServiciosTecnologicosDto>(created);
    }
}
