using MediatR;
using Backend.Models.DTOs;
using System.Collections.Generic;

namespace Backend.Queries.Tareas;

public class GetTareasByActividadQuery : IRequest<IEnumerable<TareaDto>>
{
    public int ActividadId { get; set; }
}
