using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.Entidades;

public class CreateEntidadCommand : IRequest<EntidadDto>
{
    public string Nombre { get; set; } = string.Empty;
}

public class UpdateEntidadCommand : IRequest<EntidadDto>
{
    public int EntidadId { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class DeleteEntidadCommand : IRequest<bool>
{
    public int EntidadId { get; set; }
}
