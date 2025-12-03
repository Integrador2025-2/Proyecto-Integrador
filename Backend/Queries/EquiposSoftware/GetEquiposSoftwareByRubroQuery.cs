using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.EquiposSoftware;

public class GetEquiposSoftwareByRubroQuery : IRequest<List<EquiposSoftwareDto>>
{
    public int RubroId { get; }
    public GetEquiposSoftwareByRubroQuery(int rubroId) => RubroId = rubroId;
}
