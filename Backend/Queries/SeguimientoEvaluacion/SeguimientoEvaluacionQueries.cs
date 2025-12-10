using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.SeguimientoEvaluacion;

public class GetSeguimientoEvaluacionByIdQuery : IRequest<SeguimientoEvaluacionDto>
{
    public int SeguimientoId { get; set; }
}

public class GetAllSeguimientoEvaluacionQuery : IRequest<IEnumerable<SeguimientoEvaluacionDto>>
{
}

public class GetSeguimientoEvaluacionByRecursoEspecificoIdQuery : IRequest<SeguimientoEvaluacionDto?>
{
    public int RecursoEspecificoId { get; set; }
}
