using MediatR;

namespace Backend.Commands.EquiposSoftware;

public class DeleteEquiposSoftwareCommand : IRequest<bool>
{
    public int EquiposSoftwareId { get; }
    public DeleteEquiposSoftwareCommand(int id) => EquiposSoftwareId = id;
}
