using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.ActxEntidad;

public class GetActxEntidadByIdQuery : IRequest<ActxEntidadDto?>
{
    public int Id { get; set; }
}
