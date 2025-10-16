using MediatR;
using Backend.Commands.Tareas;
using Backend.Infrastructure.Repositories;

namespace Backend.Handlers.Commands;

public class DeleteTareaCommandHandler : IRequestHandler<DeleteTareaCommand, bool>
{
    private readonly ITareaRepository _repo;

    public DeleteTareaCommandHandler(ITareaRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> Handle(DeleteTareaCommand request, CancellationToken cancellationToken)
    {
    return await _repo.DeleteAsync(request.TareaId);
    }
}
