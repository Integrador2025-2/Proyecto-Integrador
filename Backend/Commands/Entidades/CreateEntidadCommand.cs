using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.Entidades;

public class CreateEntidadCommand : IRequest<EntidadDto>
{
    public string Nombre { get; set; } = string.Empty;
}
