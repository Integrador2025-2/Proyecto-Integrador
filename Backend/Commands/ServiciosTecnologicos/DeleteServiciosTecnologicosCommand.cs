using MediatR;

namespace Backend.Commands.ServiciosTecnologicos;

public class DeleteServiciosTecnologicosCommand : IRequest<bool>
{
    public int ServiciosTecnologicosId { get; }
    public DeleteServiciosTecnologicosCommand(int id) => ServiciosTecnologicosId = id;
}
