using Backend.Commands.Proyectos;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateProyectoHandler : IRequestHandler<CreateProyectoCommand, ProyectoDto>
{
    private readonly IProyectoRepository _proyectoRepository;

    public CreateProyectoHandler(IProyectoRepository proyectoRepository)
    {
        _proyectoRepository = proyectoRepository;
    }

    public async Task<ProyectoDto> Handle(CreateProyectoCommand request, CancellationToken cancellationToken)
    {
        var proyecto = new Proyecto
        {
            Nombre = "Nuevo Proyecto",
            FechaCreacion = DateTime.UtcNow,
            Estado = "Activo",
            UsuarioId = request.UsuarioId
        };

        var createdProyecto = await _proyectoRepository.CreateAsync(proyecto);

        return new ProyectoDto
        {
            ProyectoId = createdProyecto.ProyectoId,
            FechaCreacion = createdProyecto.FechaCreacion,
            UsuarioId = createdProyecto.UsuarioId
        };
    }
}

public class UpdateProyectoHandler : IRequestHandler<UpdateProyectoCommand, ProyectoDto>
{
    private readonly IProyectoRepository _proyectoRepository;

    public UpdateProyectoHandler(IProyectoRepository proyectoRepository)
    {
        _proyectoRepository = proyectoRepository;
    }

    public async Task<ProyectoDto> Handle(UpdateProyectoCommand request, CancellationToken cancellationToken)
    {
        var proyecto = await _proyectoRepository.GetByIdAsync(request.ProyectoId);
        if (proyecto == null)
            throw new KeyNotFoundException($"Proyecto with ID {request.ProyectoId} not found");

        proyecto.UsuarioId = request.UsuarioId;

        var updatedProyecto = await _proyectoRepository.UpdateAsync(proyecto);

        return new ProyectoDto
        {
            ProyectoId = updatedProyecto.ProyectoId,
            FechaCreacion = updatedProyecto.FechaCreacion,
            UsuarioId = updatedProyecto.UsuarioId
        };
    }
}

public class DeleteProyectoHandler : IRequestHandler<DeleteProyectoCommand, bool>
{
    private readonly IProyectoRepository _proyectoRepository;

    public DeleteProyectoHandler(IProyectoRepository proyectoRepository)
    {
        _proyectoRepository = proyectoRepository;
    }

    public async Task<bool> Handle(DeleteProyectoCommand request, CancellationToken cancellationToken)
    {
        var exists = await _proyectoRepository.ExistsAsync(request.ProyectoId);
        if (!exists)
            return false;

        await _proyectoRepository.DeleteAsync(request.ProyectoId);
        return true;
    }
}
