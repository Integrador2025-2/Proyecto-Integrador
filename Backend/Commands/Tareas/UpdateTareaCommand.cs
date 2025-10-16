using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.Tareas;

public class UpdateTareaCommand : IRequest<TareaDto>
{
    public int TareaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Periodo { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public int ActividadId { get; set; }
}
