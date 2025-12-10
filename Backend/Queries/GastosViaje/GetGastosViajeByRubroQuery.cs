using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.GastosViaje;

public class GetGastosViajeByRubroQuery : IRequest<List<GastosViajeDto>>
{
    public int RubroId { get; }
    public GetGastosViajeByRubroQuery(int rubroId) => RubroId = rubroId;
}
