using AutoMapper;
using Backend.Commands.ServiciosTecnologicos;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateServiciosTecnologicosHandler : IRequestHandler<CreateServiciosTecnologicosCommand, ServiciosTecnologicosDto>
{
    private readonly IServiciosTecnologicosRepository _repository;
    private readonly IMapper _mapper;

    public CreateServiciosTecnologicosHandler(IServiciosTecnologicosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ServiciosTecnologicosDto> Handle(CreateServiciosTecnologicosCommand request, CancellationToken cancellationToken)
    {
        var serviciosTecnologicos = new ServiciosTecnologicos
        {
            RecursoEspecificoId = request.RecursoEspecificoId,
            Descripcion = request.Descripcion
        };

        var created = await _repository.CreateAsync(serviciosTecnologicos);
        return _mapper.Map<ServiciosTecnologicosDto>(created);
    }
}

public class UpdateServiciosTecnologicosHandler : IRequestHandler<UpdateServiciosTecnologicosCommand, ServiciosTecnologicosDto>
{
    private readonly IServiciosTecnologicosRepository _repository;
    private readonly IMapper _mapper;

    public UpdateServiciosTecnologicosHandler(IServiciosTecnologicosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ServiciosTecnologicosDto> Handle(UpdateServiciosTecnologicosCommand request, CancellationToken cancellationToken)
    {
        var serviciosTecnologicos = await _repository.GetByIdAsync(request.ServiciosTecnologicosId);
        
        if (serviciosTecnologicos == null)
        {
            throw new KeyNotFoundException($"ServiciosTecnologicos with ID {request.ServiciosTecnologicosId} not found");
        }

        serviciosTecnologicos.Descripcion = request.Descripcion;

        var updated = await _repository.UpdateAsync(serviciosTecnologicos);
        return _mapper.Map<ServiciosTecnologicosDto>(updated);
    }
}

public class DeleteServiciosTecnologicosHandler : IRequestHandler<DeleteServiciosTecnologicosCommand, bool>
{
    private readonly IServiciosTecnologicosRepository _repository;

    public DeleteServiciosTecnologicosHandler(IServiciosTecnologicosRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteServiciosTecnologicosCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.ServiciosTecnologicosId);
    }
}
