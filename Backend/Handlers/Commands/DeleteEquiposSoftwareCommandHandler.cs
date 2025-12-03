using Backend.Commands.EquiposSoftware;
using Backend.Infrastructure.Repositories;
using MediatR;

namespace Backend.Handlers.Commands;

public class DeleteEquiposSoftwareCommandHandler : IRequestHandler<DeleteEquiposSoftwareCommand, bool>
{
    private readonly IEquiposSoftwareRepository _repo;
    public DeleteEquiposSoftwareCommandHandler(IEquiposSoftwareRepository repo) { _repo = repo; }
    public async Task<bool> Handle(DeleteEquiposSoftwareCommand request, CancellationToken cancellationToken)
    {
        return await _repo.DeleteAsync(request.EquiposSoftwareId);
    }
}
