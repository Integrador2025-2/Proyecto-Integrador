using AutoMapper;
using Backend.Commands.EquiposSoftware;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateEquiposSoftwareHandler : IRequestHandler<CreateEquiposSoftwareCommand, EquiposSoftwareDto>
{
    private readonly IEquiposSoftwareRepository _repository;
    private readonly IMapper _mapper;

    public CreateEquiposSoftwareHandler(IEquiposSoftwareRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EquiposSoftwareDto> Handle(CreateEquiposSoftwareCommand request, CancellationToken cancellationToken)
    {
        var equiposSoftware = new EquiposSoftware
        {
            RecursoEspecificoId = request.RecursoEspecificoId,
            EspecificacionesTecnicas = request.EspecificacionesTecnicas
        };

        var created = await _repository.CreateAsync(equiposSoftware);
        return _mapper.Map<EquiposSoftwareDto>(created);
    }
}

public class UpdateEquiposSoftwareHandler : IRequestHandler<UpdateEquiposSoftwareCommand, EquiposSoftwareDto>
{
    private readonly IEquiposSoftwareRepository _repository;
    private readonly IMapper _mapper;

    public UpdateEquiposSoftwareHandler(IEquiposSoftwareRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EquiposSoftwareDto> Handle(UpdateEquiposSoftwareCommand request, CancellationToken cancellationToken)
    {
        var equiposSoftware = await _repository.GetByIdAsync(request.EquiposSoftwareId);
        
        if (equiposSoftware == null)
        {
            throw new KeyNotFoundException($"EquiposSoftware with ID {request.EquiposSoftwareId} not found");
        }

        equiposSoftware.EspecificacionesTecnicas = request.EspecificacionesTecnicas;

        var updated = await _repository.UpdateAsync(equiposSoftware);
        return _mapper.Map<EquiposSoftwareDto>(updated);
    }
}

public class DeleteEquiposSoftwareHandler : IRequestHandler<DeleteEquiposSoftwareCommand, bool>
{
    private readonly IEquiposSoftwareRepository _repository;

    public DeleteEquiposSoftwareHandler(IEquiposSoftwareRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteEquiposSoftwareCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.EquiposSoftwareId);
    }
}
