using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.Entidades;

public class UpdateEntidadCommand : IRequest<EntidadDto>
{
    public int EntidadId { get; set; }
    public string Nombre { get; set; } = string.Empty;
}
