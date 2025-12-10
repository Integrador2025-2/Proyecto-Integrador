using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.CapacitacionEventos;

public class GetCapacitacionEventosByIdQuery : IRequest<CapacitacionEventosDto>
{
    public int CapacitacionEventosId { get; set; }
}

public class GetAllCapacitacionEventosQuery : IRequest<IEnumerable<CapacitacionEventosDto>>
{
}

public class GetCapacitacionEventosByRecursoEspecificoIdQuery : IRequest<CapacitacionEventosDto?>
{
    public int RecursoEspecificoId { get; set; }
}
