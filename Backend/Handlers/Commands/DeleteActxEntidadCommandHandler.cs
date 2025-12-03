using Backend.Commands.ActxEntidad;
using Backend.Infrastructure.Repositories;
using MediatR;

namespace Backend.Handlers.Commands;

public class DeleteActxEntidadCommandHandler : IRequestHandler<DeleteActxEntidadCommand, Unit>
{
    private readonly IActXEntidadRepository _repo;

    public DeleteActxEntidadCommandHandler(IActXEntidadRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(DeleteActxEntidadCommand request, CancellationToken cancellationToken)
    {
        var deleted = await _repo.DeleteAsync(request.ActXEntidadId);
        if (!deleted) throw new KeyNotFoundException("ActXEntidad not found");
        return Unit.Value;
    }
}
