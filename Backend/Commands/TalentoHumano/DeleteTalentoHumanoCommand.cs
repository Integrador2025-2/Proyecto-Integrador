using MediatR;

namespace Backend.Commands.TalentoHumano;

public class DeleteTalentoHumanoCommand : IRequest<bool>
{
    public int TalentoHumanoId { get; }
    public DeleteTalentoHumanoCommand(int id) => TalentoHumanoId = id;
}
