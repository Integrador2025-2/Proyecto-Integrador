using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.ProteccionConocimientoDivulgacion;

public class CreateProteccionConocimientoDivulgacionCommand : IRequest<ProteccionConocimientoDivulgacionDto>
{
    public int RecursoEspecificoId { get; set; }
    public string ActividadHapat { get; set; } = string.Empty;
    public string EntidadResponsable { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
}

public class UpdateProteccionConocimientoDivulgacionCommand : IRequest<ProteccionConocimientoDivulgacionDto>
{
    public int ProteccionId { get; set; }
    public string ActividadHapat { get; set; } = string.Empty;
    public string EntidadResponsable { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
}

public class DeleteProteccionConocimientoDivulgacionCommand : IRequest<bool>
{
    public int ProteccionId { get; set; }
}
