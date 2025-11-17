using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.CapacitacionEventos;

public class CreateCapacitacionEventosCommand : IRequest<CapacitacionEventosDto>
{
    public int RecursoEspecificoId { get; set; }
    public string Tema { get; set; } = string.Empty;
    public int Cantidad { get; set; }
}

public class UpdateCapacitacionEventosCommand : IRequest<CapacitacionEventosDto>
{
    public int CapacitacionEventosId { get; set; }
    public string Tema { get; set; } = string.Empty;
    public int Cantidad { get; set; }
}

public class DeleteCapacitacionEventosCommand : IRequest<bool>
{
    public int CapacitacionEventosId { get; set; }
}
