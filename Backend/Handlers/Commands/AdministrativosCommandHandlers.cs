using AutoMapper;
using Backend.Commands.Administrativos;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateAdministrativosHandler : IRequestHandler<CreateAdministrativosCommand, AdministrativosDto>
{
    private readonly IAdministrativosRepository _repository;
    private readonly IMapper _mapper;

    public CreateAdministrativosHandler(IAdministrativosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AdministrativosDto> Handle(CreateAdministrativosCommand request, CancellationToken cancellationToken)
    {
        var administrativos = new Administrativos
        {
            RecursoEspecificoId = request.RecursoEspecificoId,
            Cargo = request.Cargo,
            RazonSocial = request.RazonSocial,
            Justificacion = request.Justificacion
        };

        var created = await _repository.CreateAsync(administrativos);
        return _mapper.Map<AdministrativosDto>(created);
    }
}

public class UpdateAdministrativosHandler : IRequestHandler<UpdateAdministrativosCommand, AdministrativosDto>
{
    private readonly IAdministrativosRepository _repository;
    private readonly IMapper _mapper;

    public UpdateAdministrativosHandler(IAdministrativosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AdministrativosDto> Handle(UpdateAdministrativosCommand request, CancellationToken cancellationToken)
    {
        var administrativos = await _repository.GetByIdAsync(request.AdministrativoId);
        
        if (administrativos == null)
        {
            throw new KeyNotFoundException($"Administrativos with ID {request.AdministrativoId} not found");
        }

        administrativos.Cargo = request.Cargo;
        administrativos.RazonSocial = request.RazonSocial;
        administrativos.Justificacion = request.Justificacion;

        var updated = await _repository.UpdateAsync(administrativos);
        return _mapper.Map<AdministrativosDto>(updated);
    }
}

public class DeleteAdministrativosHandler : IRequestHandler<DeleteAdministrativosCommand, bool>
{
    private readonly IAdministrativosRepository _repository;

    public DeleteAdministrativosHandler(IAdministrativosRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteAdministrativosCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.AdministrativoId);
    }
}
