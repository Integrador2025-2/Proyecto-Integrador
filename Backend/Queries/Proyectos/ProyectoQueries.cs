using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Proyectos;

public class GetProyectoByIdQuery : IRequest<ProyectoDto?>
{
    public int ProyectoId { get; set; }
}

public class GetAllProyectosQuery : IRequest<IEnumerable<ProyectoDto>>
{
}

public class GetProyectosByUsuarioIdQuery : IRequest<IEnumerable<ProyectoDto>>
{
    public int UsuarioId { get; set; }
}
