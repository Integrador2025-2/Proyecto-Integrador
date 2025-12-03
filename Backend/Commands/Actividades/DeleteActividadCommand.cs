using MediatR;

namespace Backend.Commands.Actividades;

public class DeleteActividadCommand : IRequest<bool>
{
    public int ActividadId { get; }
    public DeleteActividadCommand(int actividadId) => ActividadId = actividadId;
}
