using Backend.Commands.Entidades;
using Backend.Infrastructure.Repositories;
using MediatR;

namespace Backend.Handlers.Commands;

public class DeleteEntidadCommandHandler : IRequestHandler<DeleteEntidadCommand, Unit>
{
    private readonly IEntidadRepository _repo;

    public DeleteEntidadCommandHandler(IEntidadRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(DeleteEntidadCommand request, CancellationToken cancellationToken)
    {
        var deleted = await _repo.DeleteAsync(request.EntidadId);
        if (!deleted) throw new KeyNotFoundException("Entidad not found");
        return Unit.Value;
    }
}
