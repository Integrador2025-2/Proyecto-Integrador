using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.Tareas;

public class CreateTareaCommand : IRequest<TareaDto>
{
    public int ActividadId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Periodo { get; set; } = string.Empty;
    public decimal Monto { get; set; }
}

public class UpdateTareaCommand : IRequest<TareaDto>
{
    public int TareaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Periodo { get; set; } = string.Empty;
    public decimal Monto { get; set; }
}

public class DeleteTareaCommand : IRequest<bool>
{
    public int TareaId { get; set; }
}
