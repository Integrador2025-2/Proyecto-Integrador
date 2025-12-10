using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Rubros;

public class GetRubroByIdQuery : IRequest<RubroDto?>
{
    public int RubroId { get; set; }
}

public class GetAllRubrosQuery : IRequest<IEnumerable<RubroDto>>
{
}
