using Backend.Commands.ServiciosTecnologicos;
using Backend.Infrastructure.Repositories;
using MediatR;

namespace Backend.Handlers.Commands;

public class DeleteServiciosTecnologicosCommandHandler : IRequestHandler<DeleteServiciosTecnologicosCommand, bool>
{
    private readonly IServiciosTecnologicosRepository _repo;
    public DeleteServiciosTecnologicosCommandHandler(IServiciosTecnologicosRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteServiciosTecnologicosCommand request, CancellationToken cancellationToken)
    {
        return await _repo.DeleteAsync(request.ServiciosTecnologicosId);
    }
}
