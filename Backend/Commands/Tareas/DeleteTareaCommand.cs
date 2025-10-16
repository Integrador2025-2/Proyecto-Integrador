using MediatR;

namespace Backend.Commands.Tareas;

public class DeleteTareaCommand : IRequest<bool>
{
    public int TareaId { get; set; }
}
