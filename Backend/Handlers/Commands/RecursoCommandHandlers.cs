using Backend.Commands.Recursos;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateRecursoHandler : IRequestHandler<CreateRecursoCommand, RecursoDto>
{
    private readonly IRecursoRepository _recursoRepository;

    public CreateRecursoHandler(IRecursoRepository recursoRepository)
    {
        _recursoRepository = recursoRepository;
    }

    public async Task<RecursoDto> Handle(CreateRecursoCommand request, CancellationToken cancellationToken)
    {
        var recurso = new Recurso
        {
            ActividadId = request.ActividadId,
            EntidadId = request.EntidadId,
            RubroId = request.RubroId,
            TipoRecurso = request.TipoRecurso,
            MontoEfectivo = request.MontoEfectivo,
            MontoEspecie = request.MontoEspecie,
            Descripcion = request.Descripcion
        };

        var createdRecurso = await _recursoRepository.CreateAsync(recurso);

        return new RecursoDto
        {
            RecursoId = createdRecurso.RecursoId,
            ActividadId = createdRecurso.ActividadId,
            EntidadId = createdRecurso.EntidadId,
            RubroId = createdRecurso.RubroId,
            TipoRecurso = createdRecurso.TipoRecurso,
            MontoEfectivo = createdRecurso.MontoEfectivo,
            MontoEspecie = createdRecurso.MontoEspecie,
            Descripcion = createdRecurso.Descripcion
        };
    }
}

public class UpdateRecursoHandler : IRequestHandler<UpdateRecursoCommand, RecursoDto>
{
    private readonly IRecursoRepository _recursoRepository;

    public UpdateRecursoHandler(IRecursoRepository recursoRepository)
    {
        _recursoRepository = recursoRepository;
    }

    public async Task<RecursoDto> Handle(UpdateRecursoCommand request, CancellationToken cancellationToken)
    {
        var recurso = await _recursoRepository.GetByIdAsync(request.RecursoId);
        if (recurso == null)
            throw new KeyNotFoundException($"Recurso with ID {request.RecursoId} not found");

        recurso.EntidadId = request.EntidadId;
        recurso.RubroId = request.RubroId;
        recurso.TipoRecurso = request.TipoRecurso;
        recurso.MontoEfectivo = request.MontoEfectivo;
        recurso.MontoEspecie = request.MontoEspecie;
        recurso.Descripcion = request.Descripcion;

        var updatedRecurso = await _recursoRepository.UpdateAsync(recurso);

        return new RecursoDto
        {
            RecursoId = updatedRecurso.RecursoId,
            ActividadId = updatedRecurso.ActividadId,
            EntidadId = updatedRecurso.EntidadId,
            RubroId = updatedRecurso.RubroId,
            TipoRecurso = updatedRecurso.TipoRecurso,
            MontoEfectivo = updatedRecurso.MontoEfectivo,
            MontoEspecie = updatedRecurso.MontoEspecie,
            Descripcion = updatedRecurso.Descripcion
        };
    }
}

public class DeleteRecursoHandler : IRequestHandler<DeleteRecursoCommand, bool>
{
    private readonly IRecursoRepository _recursoRepository;

    public DeleteRecursoHandler(IRecursoRepository recursoRepository)
    {
        _recursoRepository = recursoRepository;
    }

    public async Task<bool> Handle(DeleteRecursoCommand request, CancellationToken cancellationToken)
    {
        var exists = await _recursoRepository.ExistsAsync(request.RecursoId);
        if (!exists)
            return false;

        await _recursoRepository.DeleteAsync(request.RecursoId);
        return true;
    }
}
