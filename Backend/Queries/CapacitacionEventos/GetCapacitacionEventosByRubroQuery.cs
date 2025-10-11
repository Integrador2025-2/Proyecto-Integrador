using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.CapacitacionEventos;

public class GetCapacitacionEventosByRubroQuery : IRequest<List<CapacitacionEventosDto>>
{
    public int RubroId { get; }
    public GetCapacitacionEventosByRubroQuery(int rubroId) => RubroId = rubroId;
}
