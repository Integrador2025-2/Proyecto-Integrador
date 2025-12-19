using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.MaterialesInsumos;

public class CreateMaterialesInsumosCommand : IRequest<MaterialesInsumosDto>
{
    public int RubroId { get; set; }
    public string Materiales { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
    public int? ActividadId { get; set; }
}
