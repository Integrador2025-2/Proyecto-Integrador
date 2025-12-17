using AutoMapper;
using Backend.Commands.Contratacion;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateContratacionHandler : IRequestHandler<CreateContratacionCommand, ContratacionDto>
{
    private readonly IContratacionRepository _repository;
    private readonly IMapper _mapper;

    public CreateContratacionHandler(IContratacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ContratacionDto> Handle(CreateContratacionCommand request, CancellationToken cancellationToken)
    {
        var contratacion = new Contratacion
        {
            NivelGestion = request.NivelGestion,
            Categoria = request.Categoria,
            IdentidadAcademica = request.IdentidadAcademica,
            ExperienciaMinima = request.ExperienciaMinima,
            Iva = request.Iva,
            ValorMensual = request.ValorMensual
        };

        var created = await _repository.CreateAsync(contratacion);
        return _mapper.Map<ContratacionDto>(created);
    }
}

public class UpdateContratacionHandler : IRequestHandler<UpdateContratacionCommand, ContratacionDto>
{
    private readonly IContratacionRepository _repository;
    private readonly IMapper _mapper;

    public UpdateContratacionHandler(IContratacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ContratacionDto> Handle(UpdateContratacionCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.ContratacionId);
        if (existing == null)
        {
            throw new KeyNotFoundException($"Contratacion with ID {request.ContratacionId} not found.");
        }

        existing.NivelGestion = request.NivelGestion;
        existing.Categoria = request.Categoria;
        existing.IdentidadAcademica = request.IdentidadAcademica;
        existing.ExperienciaMinima = request.ExperienciaMinima;
        existing.Iva = request.Iva;
        existing.ValorMensual = request.ValorMensual;

        var updated = await _repository.UpdateAsync(existing);
        return _mapper.Map<ContratacionDto>(updated);
    }
}

public class DeleteContratacionHandler : IRequestHandler<DeleteContratacionCommand>
{
    private readonly IContratacionRepository _repository;

    public DeleteContratacionHandler(IContratacionRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteContratacionCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.ContratacionId);
    }
}
