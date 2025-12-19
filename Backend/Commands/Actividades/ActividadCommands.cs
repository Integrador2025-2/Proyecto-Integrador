using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.Actividades;

public class CreateActividadCommand : IRequest<ActividadDto>
{
    public int CadenaDeValorId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
    public int DuracionAnios { get; set; }
    public string EspecificacionesTecnicas { get; set; } = string.Empty;
    public decimal ValorUnitario { get; set; }
}

public class UpdateActividadCommand : IRequest<ActividadDto>
{
    public int ActividadId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
    public int DuracionAnios { get; set; }
    public string EspecificacionesTecnicas { get; set; } = string.Empty;
    public decimal ValorUnitario { get; set; }
}

public class DeleteActividadCommand : IRequest<bool>
{
    public int ActividadId { get; set; }
}
