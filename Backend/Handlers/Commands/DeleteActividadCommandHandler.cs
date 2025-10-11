using Backend.Commands.Actividades;
using Backend.Infrastructure.Repositories;
using MediatR;

namespace Backend.Handlers.Commands;

public class DeleteActividadCommandHandler : IRequestHandler<DeleteActividadCommand, bool>
{
    private readonly IActividadRepository _actividadRepository;
    public DeleteActividadCommandHandler(IActividadRepository actividadRepository)
    {
        _actividadRepository = actividadRepository;
    }

    public async Task<bool> Handle(DeleteActividadCommand request, CancellationToken cancellationToken)
    {
        return await _actividadRepository.DeleteAsync(request.ActividadId);
    }
}
