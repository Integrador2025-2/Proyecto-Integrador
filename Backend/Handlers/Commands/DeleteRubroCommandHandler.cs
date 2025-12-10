using Backend.Commands.Rubros;
using Backend.Infrastructure.Repositories;
using MediatR;

namespace Backend.Handlers.Commands;

public class DeleteRubroCommandHandler : IRequestHandler<DeleteRubroCommand, bool>
{
    private readonly IRubroRepository _rubroRepository;
    public DeleteRubroCommandHandler(IRubroRepository rubroRepository) => _rubroRepository = rubroRepository;

    public async Task<bool> Handle(DeleteRubroCommand request, CancellationToken cancellationToken)
    {
        return await _rubroRepository.DeleteAsync(request.RubroId);
    }
}
