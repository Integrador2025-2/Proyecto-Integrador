using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.ServiciosTecnologicos;

public class GetServiciosByRubroQuery : IRequest<List<ServiciosTecnologicosDto>>
{
    public int RubroId { get; }
    public GetServiciosByRubroQuery(int rubroId) => RubroId = rubroId;
}
