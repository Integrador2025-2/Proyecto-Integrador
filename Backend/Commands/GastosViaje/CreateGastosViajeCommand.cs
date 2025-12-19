using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.GastosViaje;

public class CreateGastosViajeCommand : IRequest<GastosViajeDto>
{
    public int RubroId { get; set; }
    public decimal Costo { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
    public int? ActividadId { get; set; }
}
