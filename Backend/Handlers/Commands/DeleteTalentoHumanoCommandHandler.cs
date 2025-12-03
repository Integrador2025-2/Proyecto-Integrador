using Backend.Commands.TalentoHumano;
using Backend.Infrastructure.Repositories;
using MediatR;

namespace Backend.Handlers.Commands;

public class DeleteTalentoHumanoCommandHandler : IRequestHandler<DeleteTalentoHumanoCommand, bool>
{
    private readonly ITalentoHumanoRepository _repo;
    public DeleteTalentoHumanoCommandHandler(ITalentoHumanoRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteTalentoHumanoCommand request, CancellationToken cancellationToken)
    {
        return await _repo.DeleteAsync(request.TalentoHumanoId);
    }
}
