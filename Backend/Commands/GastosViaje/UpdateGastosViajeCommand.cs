using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.GastosViaje;

public class UpdateGastosViajeCommand : IRequest<GastosViajeDto?>
{
    public int GastosViajeId { get; set; }
    public int RubroId { get; set; }
    public decimal Costo { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}
