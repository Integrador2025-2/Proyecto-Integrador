using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.ServiciosTecnologicos;

public class CreateServiciosTecnologicosCommand : IRequest<ServiciosTecnologicosDto>
{
    public int RubroId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}
