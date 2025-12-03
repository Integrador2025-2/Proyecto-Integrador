using MediatR;

namespace Backend.Commands.GastosViaje;

public class DeleteGastosViajeCommand : IRequest<bool>
{
    public int GastosViajeId { get; }
    public DeleteGastosViajeCommand(int id) => GastosViajeId = id;
}
