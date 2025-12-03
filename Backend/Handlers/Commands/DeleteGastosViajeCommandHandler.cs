using Backend.Commands.GastosViaje;
using Backend.Infrastructure.Repositories;
using MediatR;

namespace Backend.Handlers.Commands;

public class DeleteGastosViajeCommandHandler : IRequestHandler<DeleteGastosViajeCommand, bool>
{
    private readonly IGastosViajeRepository _repo;
    public DeleteGastosViajeCommandHandler(IGastosViajeRepository repo) { _repo = repo; }
    public async Task<bool> Handle(DeleteGastosViajeCommand request, CancellationToken cancellationToken)
    {
        return await _repo.DeleteAsync(request.GastosViajeId);
    }
}
