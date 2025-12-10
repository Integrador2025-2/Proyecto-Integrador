using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.MaterialesInsumos;

public class GetMaterialesInsumosByIdQuery : IRequest<MaterialesInsumosDto>
{
    public int MaterialesInsumosId { get; set; }
}

public class GetAllMaterialesInsumosQuery : IRequest<IEnumerable<MaterialesInsumosDto>>
{
}

public class GetMaterialesInsumosByRecursoEspecificoIdQuery : IRequest<MaterialesInsumosDto?>
{
    public int RecursoEspecificoId { get; set; }
}
