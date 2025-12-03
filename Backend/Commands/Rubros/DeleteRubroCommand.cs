using MediatR;

namespace Backend.Commands.Rubros;

public class DeleteRubroCommand : IRequest<bool>
{
    public int RubroId { get; }
    public DeleteRubroCommand(int rubroId) => RubroId = rubroId;
}
