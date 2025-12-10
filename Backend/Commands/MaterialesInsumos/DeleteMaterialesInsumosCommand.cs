using MediatR;

namespace Backend.Commands.MaterialesInsumos;

public class DeleteMaterialesInsumosCommand : IRequest<bool>
{
    public int MaterialesInsumosId { get; }
    public DeleteMaterialesInsumosCommand(int id) => MaterialesInsumosId = id;
}
