using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.EquiposSoftware;

public class UpdateEquiposSoftwareCommand : IRequest<EquiposSoftwareDto?>
{
    public int EquiposSoftwareId { get; set; }
    public int RubroId { get; set; }
    public string EspecificacionesTecnicas { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}
