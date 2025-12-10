using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.Rubros;

public class GetRubroByIdQuery : IRequest<RubroDto?>
{
    public int RubroId { get; }
    public GetRubroByIdQuery(int rubroId) => RubroId = rubroId;
}
