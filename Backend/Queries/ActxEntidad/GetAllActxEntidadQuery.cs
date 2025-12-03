using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.ActxEntidad;

public class GetAllActxEntidadQuery : IRequest<List<ActxEntidadDto>>
{
    public int? ActividadId { get; set; }
}
