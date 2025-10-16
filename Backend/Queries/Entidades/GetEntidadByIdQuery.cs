using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Entidades;

public class GetEntidadByIdQuery : IRequest<EntidadDto?>
{
    public int Id { get; set; }
}
