using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.ActxEntidad;

public class UpdateActxEntidadCommand : IRequest<ActxEntidadDto>
{
    public int ActXEntidadId { get; set; }
    public int ActividadId { get; set; }
    public int EntidadId { get; set; }
    public decimal Efectivo { get; set; }
    public decimal Especie { get; set; }
}
