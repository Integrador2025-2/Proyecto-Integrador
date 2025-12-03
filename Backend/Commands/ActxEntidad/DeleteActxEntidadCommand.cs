using MediatR;

namespace Backend.Commands.ActxEntidad;

public class DeleteActxEntidadCommand : IRequest<Unit>
{
    public int ActXEntidadId { get; set; }
}
