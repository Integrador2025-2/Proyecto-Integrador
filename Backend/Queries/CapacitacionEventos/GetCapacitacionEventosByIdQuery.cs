using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.CapacitacionEventos;

public class GetCapacitacionEventosByIdQuery : IRequest<CapacitacionEventosDto?>
{
    public int Id { get; }
    public GetCapacitacionEventosByIdQuery(int id) => Id = id;
}
