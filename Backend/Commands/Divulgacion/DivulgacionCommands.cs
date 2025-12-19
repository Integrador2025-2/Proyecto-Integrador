using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.Divulgacion;

public class CreateDivulgacionCommand : IRequest<DivulgacionDto>
{
    public int RecursoEspecificoId { get; set; }
    public string MedioDivulgacion { get; set; } = string.Empty;
    public string Alcance { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
}

public class UpdateDivulgacionCommand : IRequest<DivulgacionDto>
{
    public int DivulgacionId { get; set; }
    public string MedioDivulgacion { get; set; } = string.Empty;
    public string Alcance { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
}

public class DeleteDivulgacionCommand : IRequest<bool>
{
    public int DivulgacionId { get; set; }
}
