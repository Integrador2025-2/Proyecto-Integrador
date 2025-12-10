using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Entidades;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetEntidadByIdHandler : IRequestHandler<GetEntidadByIdQuery, EntidadDto?>
{
    private readonly IEntidadRepository _entidadRepository;

    public GetEntidadByIdHandler(IEntidadRepository entidadRepository)
    {
        _entidadRepository = entidadRepository;
    }

    public async Task<EntidadDto?> Handle(GetEntidadByIdQuery request, CancellationToken cancellationToken)
    {
        var entidad = await _entidadRepository.GetByIdAsync(request.EntidadId);
        if (entidad == null)
            return null;

        return new EntidadDto
        {
            EntidadId = entidad.EntidadId,
            Nombre = entidad.Nombre
        };
    }
}

public class GetAllEntidadesHandler : IRequestHandler<GetAllEntidadesQuery, IEnumerable<EntidadDto>>
{
    private readonly IEntidadRepository _entidadRepository;

    public GetAllEntidadesHandler(IEntidadRepository entidadRepository)
    {
        _entidadRepository = entidadRepository;
    }

    public async Task<IEnumerable<EntidadDto>> Handle(GetAllEntidadesQuery request, CancellationToken cancellationToken)
    {
        var entidades = await _entidadRepository.GetAllAsync();

        return entidades.Select(e => new EntidadDto
        {
            EntidadId = e.EntidadId,
            Nombre = e.Nombre
        });
    }
}
