using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Tareas;

public class GetTareaByIdQuery : IRequest<TareaDto?>
{
    public int TareaId { get; set; }
}

public class GetAllTareasQuery : IRequest<IEnumerable<TareaDto>>
{
}

public class GetTareasByActividadIdQuery : IRequest<IEnumerable<TareaDto>>
{
    public int ActividadId { get; set; }
}
