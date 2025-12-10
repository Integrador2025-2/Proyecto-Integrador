using AutoMapper;
using Backend.Commands.Divulgacion;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateDivulgacionHandler : IRequestHandler<CreateDivulgacionCommand, DivulgacionDto>
{
    private readonly IDivulgacionRepository _repository;
    private readonly IMapper _mapper;

    public CreateDivulgacionHandler(IDivulgacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DivulgacionDto> Handle(CreateDivulgacionCommand request, CancellationToken cancellationToken)
    {
        var divulgacion = new Divulgacion
        {
            RecursoEspecificoId = request.RecursoEspecificoId,
            MedioDivulgacion = request.MedioDivulgacion,
            Alcance = request.Alcance,
            Justificacion = request.Justificacion
        };

        var created = await _repository.CreateAsync(divulgacion);
        return _mapper.Map<DivulgacionDto>(created);
    }
}

public class UpdateDivulgacionHandler : IRequestHandler<UpdateDivulgacionCommand, DivulgacionDto>
{
    private readonly IDivulgacionRepository _repository;
    private readonly IMapper _mapper;

    public UpdateDivulgacionHandler(IDivulgacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DivulgacionDto> Handle(UpdateDivulgacionCommand request, CancellationToken cancellationToken)
    {
        var divulgacion = await _repository.GetByIdAsync(request.DivulgacionId);
        
        if (divulgacion == null)
        {
            throw new KeyNotFoundException($"Divulgacion with ID {request.DivulgacionId} not found");
        }

        divulgacion.MedioDivulgacion = request.MedioDivulgacion;
        divulgacion.Alcance = request.Alcance;
        divulgacion.Justificacion = request.Justificacion;

        var updated = await _repository.UpdateAsync(divulgacion);
        return _mapper.Map<DivulgacionDto>(updated);
    }
}

public class DeleteDivulgacionHandler : IRequestHandler<DeleteDivulgacionCommand, bool>
{
    private readonly IDivulgacionRepository _repository;

    public DeleteDivulgacionHandler(IDivulgacionRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteDivulgacionCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.DivulgacionId);
    }
}
