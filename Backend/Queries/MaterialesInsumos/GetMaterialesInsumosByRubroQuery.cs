using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.MaterialesInsumos;

public class GetMaterialesInsumosByRubroQuery : IRequest<List<MaterialesInsumosDto>>
{
    public int RubroId { get; }
    public GetMaterialesInsumosByRubroQuery(int rubroId) => RubroId = rubroId;
}
