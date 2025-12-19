using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.EquiposSoftware;

public class CreateEquiposSoftwareCommand : IRequest<EquiposSoftwareDto>
{
    public int RecursoEspecificoId { get; set; }
    public string EspecificacionesTecnicas { get; set; } = string.Empty;
}

public class UpdateEquiposSoftwareCommand : IRequest<EquiposSoftwareDto>
{
    public int EquiposSoftwareId { get; set; }
    public string EspecificacionesTecnicas { get; set; } = string.Empty;
}

public class DeleteEquiposSoftwareCommand : IRequest<bool>
{
    public int EquiposSoftwareId { get; set; }
}
