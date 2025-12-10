using MediatR;

namespace Backend.Commands.CapacitacionEventos;

public class DeleteCapacitacionEventosCommand : IRequest<bool>
{
    public int CapacitacionEventosId { get; }
    public DeleteCapacitacionEventosCommand(int id) => CapacitacionEventosId = id;
}
