using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.Actividades;

public class CreateActividadCommand : IRequest<ActividadDto>
{
    public int ProyectoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
    public List<decimal> TotalxAnios { get; set; } = new List<decimal>();
    public int CantidadAnios { get; set; }
    public string EspecificacionesTecnicas { get; set; } = string.Empty;
    public decimal ValorUnitario { get; set; }
}
