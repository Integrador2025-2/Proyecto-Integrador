using AutoMapper;
using Backend.Commands.SeguimientoEvaluacion;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateSeguimientoEvaluacionHandler : IRequestHandler<CreateSeguimientoEvaluacionCommand, SeguimientoEvaluacionDto>
{
    private readonly ISeguimientoEvaluacionRepository _repository;
    private readonly IMapper _mapper;

    public CreateSeguimientoEvaluacionHandler(ISeguimientoEvaluacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SeguimientoEvaluacionDto> Handle(CreateSeguimientoEvaluacionCommand request, CancellationToken cancellationToken)
    {
        var seguimientoEvaluacion = new SeguimientoEvaluacion
        {
            RecursoEspecificoId = request.RecursoEspecificoId,
            CargoResponsable = request.CargoResponsable,
            MetodoEvaluacion = request.MetodoEvaluacion,
            Frecuencia = request.Frecuencia
        };

        var created = await _repository.CreateAsync(seguimientoEvaluacion);
        return _mapper.Map<SeguimientoEvaluacionDto>(created);
    }
}

public class UpdateSeguimientoEvaluacionHandler : IRequestHandler<UpdateSeguimientoEvaluacionCommand, SeguimientoEvaluacionDto>
{
    private readonly ISeguimientoEvaluacionRepository _repository;
    private readonly IMapper _mapper;

    public UpdateSeguimientoEvaluacionHandler(ISeguimientoEvaluacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SeguimientoEvaluacionDto> Handle(UpdateSeguimientoEvaluacionCommand request, CancellationToken cancellationToken)
    {
        var seguimientoEvaluacion = await _repository.GetByIdAsync(request.SeguimientoId);
        
        if (seguimientoEvaluacion == null)
        {
            throw new KeyNotFoundException($"SeguimientoEvaluacion with ID {request.SeguimientoId} not found");
        }

        seguimientoEvaluacion.CargoResponsable = request.CargoResponsable;
        seguimientoEvaluacion.MetodoEvaluacion = request.MetodoEvaluacion;
        seguimientoEvaluacion.Frecuencia = request.Frecuencia;

        var updated = await _repository.UpdateAsync(seguimientoEvaluacion);
        return _mapper.Map<SeguimientoEvaluacionDto>(updated);
    }
}

public class DeleteSeguimientoEvaluacionHandler : IRequestHandler<DeleteSeguimientoEvaluacionCommand, bool>
{
    private readonly ISeguimientoEvaluacionRepository _repository;

    public DeleteSeguimientoEvaluacionHandler(ISeguimientoEvaluacionRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteSeguimientoEvaluacionCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.SeguimientoId);
    }
}
