using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Entidades;

public class GetEntidadByIdQuery : IRequest<EntidadDto?>
{
    public int EntidadId { get; set; }
}

public class GetAllEntidadesQuery : IRequest<IEnumerable<EntidadDto>>
{
}
