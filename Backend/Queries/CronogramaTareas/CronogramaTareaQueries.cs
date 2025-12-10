using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.CronogramaTareas;

public class GetCronogramaTareaByIdQuery : IRequest<CronogramaTareaDto?>
{
    public int CronogramaId { get; set; }
}

public class GetAllCronogramaTareasQuery : IRequest<IEnumerable<CronogramaTareaDto>>
{
}

public class GetCronogramaTareasByTareaIdQuery : IRequest<IEnumerable<CronogramaTareaDto>>
{
    public int TareaId { get; set; }
}
