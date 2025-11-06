using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Proyectos;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetProyectoByIdHandler : IRequestHandler<GetProyectoByIdQuery, ProyectoDto?>
{
    private readonly IProyectoRepository _proyectoRepository;

    public GetProyectoByIdHandler(IProyectoRepository proyectoRepository)
    {
        _proyectoRepository = proyectoRepository;
    }

    public async Task<ProyectoDto?> Handle(GetProyectoByIdQuery request, CancellationToken cancellationToken)
    {
        var proyecto = await _proyectoRepository.GetByIdAsync(request.ProyectoId);
        if (proyecto == null)
            return null;

        return new ProyectoDto
        {
            ProyectoId = proyecto.ProyectoId,
            FechaCreacion = proyecto.FechaCreacion,
            UsuarioId = proyecto.UsuarioId
        };
    }
}

public class GetAllProyectosHandler : IRequestHandler<GetAllProyectosQuery, IEnumerable<ProyectoDto>>
{
    private readonly IProyectoRepository _proyectoRepository;

    public GetAllProyectosHandler(IProyectoRepository proyectoRepository)
    {
        _proyectoRepository = proyectoRepository;
    }

    public async Task<IEnumerable<ProyectoDto>> Handle(GetAllProyectosQuery request, CancellationToken cancellationToken)
    {
        var proyectos = await _proyectoRepository.GetAllAsync();

        return proyectos.Select(p => new ProyectoDto
        {
            ProyectoId = p.ProyectoId,
            FechaCreacion = p.FechaCreacion,
            UsuarioId = p.UsuarioId
        });
    }
}

public class GetProyectosByUsuarioIdHandler : IRequestHandler<GetProyectosByUsuarioIdQuery, IEnumerable<ProyectoDto>>
{
    private readonly IProyectoRepository _proyectoRepository;

    public GetProyectosByUsuarioIdHandler(IProyectoRepository proyectoRepository)
    {
        _proyectoRepository = proyectoRepository;
    }

    public async Task<IEnumerable<ProyectoDto>> Handle(GetProyectosByUsuarioIdQuery request, CancellationToken cancellationToken)
    {
        var proyectos = await _proyectoRepository.GetByUsuarioIdAsync(request.UsuarioId);

        return proyectos.Select(p => new ProyectoDto
        {
            ProyectoId = p.ProyectoId,
            FechaCreacion = p.FechaCreacion,
            UsuarioId = p.UsuarioId
        });
    }
}
