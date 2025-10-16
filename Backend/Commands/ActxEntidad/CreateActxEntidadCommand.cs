using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.ActxEntidad;

public class CreateActxEntidadCommand : IRequest<ActxEntidadDto>
{
    public int ActividadId { get; set; }
    public int EntidadId { get; set; }
    public decimal Efectivo { get; set; }
    public decimal Especie { get; set; }
}
