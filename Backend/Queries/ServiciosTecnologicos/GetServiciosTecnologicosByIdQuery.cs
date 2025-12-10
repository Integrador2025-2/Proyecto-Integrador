using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.ServiciosTecnologicos;

public class GetServiciosTecnologicosByIdQuery : IRequest<ServiciosTecnologicosDto?>
{
    public int ServiciosTecnologicosId { get; }
    public GetServiciosTecnologicosByIdQuery(int id) => ServiciosTecnologicosId = id;
}
