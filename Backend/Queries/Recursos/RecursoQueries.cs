using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Recursos;

public class GetRecursoByIdQuery : IRequest<RecursoDto?>
{
    public int RecursoId { get; set; }
}

public class GetAllRecursosQuery : IRequest<IEnumerable<RecursoDto>>
{
}

public class GetRecursosByActividadIdQuery : IRequest<IEnumerable<RecursoDto>>
{
    public int ActividadId { get; set; }
}
