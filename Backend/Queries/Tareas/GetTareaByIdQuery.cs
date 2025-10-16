using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.Tareas;

public class GetTareaByIdQuery : IRequest<TareaDto>
{
    public int TareaId { get; set; }
}
