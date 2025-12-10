using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.EquiposSoftware;

public class GetEquiposSoftwareByIdQuery : IRequest<EquiposSoftwareDto?>
{
    public int Id { get; }
    public GetEquiposSoftwareByIdQuery(int id) => Id = id;
}
