using MediatR;

namespace Backend.Commands.Entidades;

public class DeleteEntidadCommand : IRequest<Unit>
{
    public int EntidadId { get; set; }
}
