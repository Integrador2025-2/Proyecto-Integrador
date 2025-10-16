using Backend.Commands.CadenasDeValor;
using Backend.Infrastructure.Repositories;
using MediatR;

namespace Backend.Handlers.Commands;

public class DeleteCadenaDeValorCommandHandler : IRequestHandler<DeleteCadenaDeValorCommand, bool>
{
    private readonly ICadenaDeValorRepository _repo;
    public DeleteCadenaDeValorCommandHandler(ICadenaDeValorRepository repo) { _repo = repo; }
    public async Task<bool> Handle(DeleteCadenaDeValorCommand request, CancellationToken cancellationToken) => await _repo.DeleteAsync(request.CadenaDeValorId);
}
