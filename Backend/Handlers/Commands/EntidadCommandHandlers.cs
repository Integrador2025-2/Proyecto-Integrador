using Backend.Commands.Entidades;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateEntidadHandler : IRequestHandler<CreateEntidadCommand, EntidadDto>
{
    private readonly IEntidadRepository _entidadRepository;

    public CreateEntidadHandler(IEntidadRepository entidadRepository)
    {
        _entidadRepository = entidadRepository;
    }

    public async Task<EntidadDto> Handle(CreateEntidadCommand request, CancellationToken cancellationToken)
    {
        var entidad = new Entidad
        {
            Nombre = request.Nombre
        };

        var createdEntidad = await _entidadRepository.CreateAsync(entidad);

        return new EntidadDto
        {
            EntidadId = createdEntidad.EntidadId,
            Nombre = createdEntidad.Nombre
        };
    }
}

public class UpdateEntidadHandler : IRequestHandler<UpdateEntidadCommand, EntidadDto>
{
    private readonly IEntidadRepository _entidadRepository;

    public UpdateEntidadHandler(IEntidadRepository entidadRepository)
    {
        _entidadRepository = entidadRepository;
    }

    public async Task<EntidadDto> Handle(UpdateEntidadCommand request, CancellationToken cancellationToken)
    {
        var entidad = await _entidadRepository.GetByIdAsync(request.EntidadId);
        if (entidad == null)
            throw new KeyNotFoundException($"Entidad with ID {request.EntidadId} not found");

        entidad.Nombre = request.Nombre;

        var updatedEntidad = await _entidadRepository.UpdateAsync(entidad);

        return new EntidadDto
        {
            EntidadId = updatedEntidad.EntidadId,
            Nombre = updatedEntidad.Nombre
        };
    }
}

public class DeleteEntidadHandler : IRequestHandler<DeleteEntidadCommand, bool>
{
    private readonly IEntidadRepository _entidadRepository;

    public DeleteEntidadHandler(IEntidadRepository entidadRepository)
    {
        _entidadRepository = entidadRepository;
    }

    public async Task<bool> Handle(DeleteEntidadCommand request, CancellationToken cancellationToken)
    {
        var exists = await _entidadRepository.ExistsAsync(request.EntidadId);
        if (!exists)
            return false;

        await _entidadRepository.DeleteAsync(request.EntidadId);
        return true;
    }
}
