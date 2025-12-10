using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.CapacitacionEventos;

public class CreateCapacitacionEventosCommand : IRequest<CapacitacionEventosDto>
{
    public int RubroId { get; set; }
    public string Tema { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}
