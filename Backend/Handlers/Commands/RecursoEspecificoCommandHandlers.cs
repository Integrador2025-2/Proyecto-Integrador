using AutoMapper;
using Backend.Commands.RecursosEspecificos;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateRecursoEspecificoHandler : IRequestHandler<CreateRecursoEspecificoCommand, RecursoEspecificoDto>
{
    private readonly IRecursoEspecificoRepository _repository;
    private readonly IMapper _mapper;

    public CreateRecursoEspecificoHandler(IRecursoEspecificoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RecursoEspecificoDto> Handle(CreateRecursoEspecificoCommand request, CancellationToken cancellationToken)
    {
        var recursoEspecifico = new RecursoEspecifico
        {
            RecursoId = request.RecursoId,
            Tipo = request.Tipo,
            Detalle = request.Detalle,
            Cantidad = request.Cantidad,
            Total = request.Total,
            PeriodoNum = request.PeriodoNum,
            PeriodoTipo = request.PeriodoTipo
        };

        var created = await _repository.CreateAsync(recursoEspecifico);
        return _mapper.Map<RecursoEspecificoDto>(created);
    }
}

public class UpdateRecursoEspecificoHandler : IRequestHandler<UpdateRecursoEspecificoCommand, RecursoEspecificoDto>
{
    private readonly IRecursoEspecificoRepository _repository;
    private readonly IMapper _mapper;

    public UpdateRecursoEspecificoHandler(IRecursoEspecificoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RecursoEspecificoDto> Handle(UpdateRecursoEspecificoCommand request, CancellationToken cancellationToken)
    {
        var recursoEspecifico = await _repository.GetByIdAsync(request.RecursoEspecificoId);
        if (recursoEspecifico == null)
        {
            throw new KeyNotFoundException($"RecursoEspecifico con ID {request.RecursoEspecificoId} no encontrado");
        }

        recursoEspecifico.Tipo = request.Tipo;
        recursoEspecifico.Detalle = request.Detalle;
        recursoEspecifico.Cantidad = request.Cantidad;
        recursoEspecifico.Total = request.Total;
        recursoEspecifico.PeriodoNum = request.PeriodoNum;
        recursoEspecifico.PeriodoTipo = request.PeriodoTipo;

        var updated = await _repository.UpdateAsync(recursoEspecifico);
        return _mapper.Map<RecursoEspecificoDto>(updated);
    }
}

public class DeleteRecursoEspecificoHandler : IRequestHandler<DeleteRecursoEspecificoCommand, bool>
{
    private readonly IRecursoEspecificoRepository _repository;

    public DeleteRecursoEspecificoHandler(IRecursoEspecificoRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteRecursoEspecificoCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsAsync(request.RecursoEspecificoId);
        if (!exists)
        {
            throw new KeyNotFoundException($"RecursoEspecifico con ID {request.RecursoEspecificoId} no encontrado");
        }

        return await _repository.DeleteAsync(request.RecursoEspecificoId);
    }
}
