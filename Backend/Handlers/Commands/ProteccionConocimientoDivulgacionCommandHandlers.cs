using AutoMapper;
using Backend.Commands.ProteccionConocimientoDivulgacion;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateProteccionConocimientoDivulgacionHandler : IRequestHandler<CreateProteccionConocimientoDivulgacionCommand, ProteccionConocimientoDivulgacionDto>
{
    private readonly IProteccionConocimientoDivulgacionRepository _repository;
    private readonly IMapper _mapper;

    public CreateProteccionConocimientoDivulgacionHandler(IProteccionConocimientoDivulgacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProteccionConocimientoDivulgacionDto> Handle(CreateProteccionConocimientoDivulgacionCommand request, CancellationToken cancellationToken)
    {
        var proteccionConocimientoDivulgacion = new ProteccionConocimientoDivulgacion
        {
            RecursoEspecificoId = request.RecursoEspecificoId,
            ActividadHapat = request.ActividadHapat,
            EntidadResponsable = request.EntidadResponsable,
            Justificacion = request.Justificacion
        };

        var created = await _repository.CreateAsync(proteccionConocimientoDivulgacion);
        return _mapper.Map<ProteccionConocimientoDivulgacionDto>(created);
    }
}

public class UpdateProteccionConocimientoDivulgacionHandler : IRequestHandler<UpdateProteccionConocimientoDivulgacionCommand, ProteccionConocimientoDivulgacionDto>
{
    private readonly IProteccionConocimientoDivulgacionRepository _repository;
    private readonly IMapper _mapper;

    public UpdateProteccionConocimientoDivulgacionHandler(IProteccionConocimientoDivulgacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProteccionConocimientoDivulgacionDto> Handle(UpdateProteccionConocimientoDivulgacionCommand request, CancellationToken cancellationToken)
    {
        var proteccionConocimientoDivulgacion = await _repository.GetByIdAsync(request.ProteccionId);
        
        if (proteccionConocimientoDivulgacion == null)
        {
            throw new KeyNotFoundException($"ProteccionConocimientoDivulgacion with ID {request.ProteccionId} not found");
        }

        proteccionConocimientoDivulgacion.ActividadHapat = request.ActividadHapat;
        proteccionConocimientoDivulgacion.EntidadResponsable = request.EntidadResponsable;
        proteccionConocimientoDivulgacion.Justificacion = request.Justificacion;

        var updated = await _repository.UpdateAsync(proteccionConocimientoDivulgacion);
        return _mapper.Map<ProteccionConocimientoDivulgacionDto>(updated);
    }
}

public class DeleteProteccionConocimientoDivulgacionHandler : IRequestHandler<DeleteProteccionConocimientoDivulgacionCommand, bool>
{
    private readonly IProteccionConocimientoDivulgacionRepository _repository;

    public DeleteProteccionConocimientoDivulgacionHandler(IProteccionConocimientoDivulgacionRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteProteccionConocimientoDivulgacionCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.ProteccionId);
    }
}
