using AutoMapper;
using Backend.Commands.MaterialesInsumos;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateMaterialesInsumosHandler : IRequestHandler<CreateMaterialesInsumosCommand, MaterialesInsumosDto>
{
    private readonly IMaterialesInsumosRepository _repository;
    private readonly IMapper _mapper;

    public CreateMaterialesInsumosHandler(IMaterialesInsumosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<MaterialesInsumosDto> Handle(CreateMaterialesInsumosCommand request, CancellationToken cancellationToken)
    {
        var materialesInsumos = new MaterialesInsumos
        {
            RecursoEspecificoId = request.RecursoEspecificoId,
            Materiales = request.Materiales
        };

        var created = await _repository.CreateAsync(materialesInsumos);
        return _mapper.Map<MaterialesInsumosDto>(created);
    }
}

public class UpdateMaterialesInsumosHandler : IRequestHandler<UpdateMaterialesInsumosCommand, MaterialesInsumosDto>
{
    private readonly IMaterialesInsumosRepository _repository;
    private readonly IMapper _mapper;

    public UpdateMaterialesInsumosHandler(IMaterialesInsumosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<MaterialesInsumosDto> Handle(UpdateMaterialesInsumosCommand request, CancellationToken cancellationToken)
    {
        var materialesInsumos = await _repository.GetByIdAsync(request.MaterialesInsumosId);
        
        if (materialesInsumos == null)
        {
            throw new KeyNotFoundException($"MaterialesInsumos with ID {request.MaterialesInsumosId} not found");
        }

        materialesInsumos.Materiales = request.Materiales;

        var updated = await _repository.UpdateAsync(materialesInsumos);
        return _mapper.Map<MaterialesInsumosDto>(updated);
    }
}

public class DeleteMaterialesInsumosHandler : IRequestHandler<DeleteMaterialesInsumosCommand, bool>
{
    private readonly IMaterialesInsumosRepository _repository;

    public DeleteMaterialesInsumosHandler(IMaterialesInsumosRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteMaterialesInsumosCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.MaterialesInsumosId);
    }
}
