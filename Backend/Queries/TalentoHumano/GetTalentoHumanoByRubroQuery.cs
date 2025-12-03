using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.TalentoHumano;

public class GetTalentoHumanoByRubroQuery : IRequest<List<TalentoHumanoDto>>
{
    public int RubroId { get; }
    public GetTalentoHumanoByRubroQuery(int rubroId) => RubroId = rubroId;
}
