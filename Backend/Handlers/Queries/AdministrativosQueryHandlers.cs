using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Administrativos;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetAdministrativosByIdHandler : IRequestHandler<GetAdministrativosByIdQuery, AdministrativosDto>
{
    private readonly IAdministrativosRepository _repository;
    private readonly IMapper _mapper;

    public GetAdministrativosByIdHandler(IAdministrativosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AdministrativosDto> Handle(GetAdministrativosByIdQuery request, CancellationToken cancellationToken)
    {
        var administrativos = await _repository.GetByIdAsync(request.AdministrativoId);
        return _mapper.Map<AdministrativosDto>(administrativos);
    }
}

public class GetAllAdministrativosHandler : IRequestHandler<GetAllAdministrativosQuery, IEnumerable<AdministrativosDto>>
{
    private readonly IAdministrativosRepository _repository;
    private readonly IMapper _mapper;

    public GetAllAdministrativosHandler(IAdministrativosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AdministrativosDto>> Handle(GetAllAdministrativosQuery request, CancellationToken cancellationToken)
    {
        var administrativos = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<AdministrativosDto>>(administrativos);
    }
}

public class GetAdministrativosByRecursoEspecificoIdHandler : IRequestHandler<GetAdministrativosByRecursoEspecificoIdQuery, AdministrativosDto?>
{
    private readonly IAdministrativosRepository _repository;
    private readonly IMapper _mapper;

    public GetAdministrativosByRecursoEspecificoIdHandler(IAdministrativosRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AdministrativosDto?> Handle(GetAdministrativosByRecursoEspecificoIdQuery request, CancellationToken cancellationToken)
    {
        var administrativos = await _repository.GetByRecursoEspecificoIdAsync(request.RecursoEspecificoId);
        return _mapper.Map<AdministrativosDto?>(administrativos);
    }
}
