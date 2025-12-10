using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.MaterialesInsumos;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetMaterialesInsumosByIdHandler : IRequestHandler<GetMaterialesInsumosByIdQuery, MaterialesInsumosDto>
{
    private readonly IMaterialesInsumosRepository _repository;
    private readonly IMapper _mapper;

    public GetMaterialesInsumosByIdHandler(IMaterialesInsumosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<MaterialesInsumosDto> Handle(GetMaterialesInsumosByIdQuery request, CancellationToken cancellationToken)
    {
        var materialesInsumos = await _repository.GetByIdAsync(request.MaterialesInsumosId);
        return _mapper.Map<MaterialesInsumosDto>(materialesInsumos);
    }
}

public class GetAllMaterialesInsumosHandler : IRequestHandler<GetAllMaterialesInsumosQuery, IEnumerable<MaterialesInsumosDto>>
{
    private readonly IMaterialesInsumosRepository _repository;
    private readonly IMapper _mapper;

    public GetAllMaterialesInsumosHandler(IMaterialesInsumosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MaterialesInsumosDto>> Handle(GetAllMaterialesInsumosQuery request, CancellationToken cancellationToken)
    {
        var materialesInsumos = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<MaterialesInsumosDto>>(materialesInsumos);
    }
}

public class GetMaterialesInsumosByRecursoEspecificoIdHandler : IRequestHandler<GetMaterialesInsumosByRecursoEspecificoIdQuery, MaterialesInsumosDto?>
{
    private readonly IMaterialesInsumosRepository _repository;
    private readonly IMapper _mapper;

    public GetMaterialesInsumosByRecursoEspecificoIdHandler(IMaterialesInsumosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<MaterialesInsumosDto?> Handle(GetMaterialesInsumosByRecursoEspecificoIdQuery request, CancellationToken cancellationToken)
    {
        var materialesInsumos = await _repository.GetByRecursoEspecificoIdAsync(request.RecursoEspecificoId);
        return _mapper.Map<MaterialesInsumosDto?>(materialesInsumos);
    }
}
