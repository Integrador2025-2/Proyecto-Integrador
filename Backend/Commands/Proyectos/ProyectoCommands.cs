using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.Proyectos;

public class CreateProyectoCommand : IRequest<ProyectoDto>
{
    public int UsuarioId { get; set; }
}

public class UpdateProyectoCommand : IRequest<ProyectoDto>
{
    public int ProyectoId { get; set; }
    public int UsuarioId { get; set; }
}

public class DeleteProyectoCommand : IRequest<bool>
{
    public int ProyectoId { get; set; }
}

