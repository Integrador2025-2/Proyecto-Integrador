using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.TalentoHumano;

public class CreateTalentoHumanoCommand : IRequest<TalentoHumanoDto>
{
    public int RubroId { get; set; }
    public string CargoEspecifico { get; set; } = string.Empty;
    public int Semanas { get; set; }
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
    public int? ActividadId { get; set; }
}
