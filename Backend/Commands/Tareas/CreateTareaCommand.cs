using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.Tareas;

public class CreateTareaCommand : IRequest<TareaDto>
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Periodo { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public int ActividadId { get; set; }
}
