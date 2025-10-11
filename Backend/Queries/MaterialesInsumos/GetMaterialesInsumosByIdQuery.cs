using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.MaterialesInsumos;

public class GetMaterialesInsumosByIdQuery : IRequest<MaterialesInsumosDto?>
{
    public int Id { get; }
    public GetMaterialesInsumosByIdQuery(int id) => Id = id;
}
