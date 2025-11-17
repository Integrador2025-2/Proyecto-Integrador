using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.ServiciosTecnologicos;

public class GetServiciosTecnologicosByIdQuery : IRequest<ServiciosTecnologicosDto>
{
    public int ServiciosTecnologicosId { get; set; }
}

public class GetAllServiciosTecnologicosQuery : IRequest<IEnumerable<ServiciosTecnologicosDto>>
{
}

public class GetServiciosTecnologicosByRecursoEspecificoIdQuery : IRequest<ServiciosTecnologicosDto?>
{
    public int RecursoEspecificoId { get; set; }
}
