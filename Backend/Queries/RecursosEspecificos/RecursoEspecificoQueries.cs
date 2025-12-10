using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.RecursosEspecificos;

public class GetRecursoEspecificoByIdQuery : IRequest<RecursoEspecificoDto?>
{
    public int RecursoEspecificoId { get; set; }
}

public class GetAllRecursosEspecificosQuery : IRequest<IEnumerable<RecursoEspecificoDto>>
{
}

public class GetRecursosEspecificosByRecursoIdQuery : IRequest<IEnumerable<RecursoEspecificoDto>>
{
    public int RecursoId { get; set; }
}

public class GetRecursosEspecificosByTipoQuery : IRequest<IEnumerable<RecursoEspecificoDto>>
{
    public string Tipo { get; set; } = string.Empty;
}
