using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Proyectos;
using MediatR;
namespace Backend.Handlers.Queries;
public class GetAllProyectosQueryHandler : IRequestHandler<GetAllProyectosQuery, List<ProyectoDto>>
{
    private readonly IProyectoRepository _proyectoRepository;
    private readonly IMapper _mapper;

    public GetAllProyectosQueryHandler(IProyectoRepository proyectoRepository, IMapper mapper)
    {
        _proyectoRepository = proyectoRepository;
        _mapper = mapper;
    }

    public async Task<List<ProyectoDto>> Handle(GetAllProyectosQuery request, CancellationToken cancellationToken)
    {
        var proyectos = await _proyectoRepository.GetAllAsync();
        return _mapper.Map<List<ProyectoDto>>(proyectos);
    }
}