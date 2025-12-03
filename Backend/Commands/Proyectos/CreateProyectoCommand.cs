using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.Proyectos;

public class CreateProyectoCommand : IRequest<ProyectoDto>
{
    public int UsuarioId { get; set; }
}
