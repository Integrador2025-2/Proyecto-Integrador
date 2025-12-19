using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.ActxEntidad;

public class CreateActXEntidadCommand : IRequest<ActXEntidadDto>
{
    public int ActividadId { get; set; }
    public int EntidadId { get; set; }
    public decimal Efectivo { get; set; }
    public decimal Especie { get; set; }
    public decimal TotalContribucion { get; set; }
}

public class UpdateActXEntidadCommand : IRequest<ActXEntidadDto>
{
    public int Id { get; set; }
    public decimal Efectivo { get; set; }
    public decimal Especie { get; set; }
    public decimal TotalContribucion { get; set; }
}

public class DeleteActXEntidadCommand : IRequest<bool>
{
    public int Id { get; set; }
}
