using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Actividades;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetAllActividadesQueryHandler : IRequestHandler<GetAllActividadesQuery, List<ActividadDto>>
{
    private readonly IActividadRepository _actividadRepository;
    private readonly IMapper _mapper;
    public GetAllActividadesQueryHandler(IActividadRepository actividadRepository, IMapper mapper)
    {
        _actividadRepository = actividadRepository;
        _mapper = mapper;
    }

    public async Task<List<ActividadDto>> Handle(GetAllActividadesQuery request, CancellationToken cancellationToken)
    {
        var items = await _actividadRepository.GetAllAsync();
        return _mapper.Map<List<ActividadDto>>(items);
    }
}
