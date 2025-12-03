using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Models.Domain;
using Backend.Commands.Proyectos;
using MediatR;

namespace Backend.Handlers.Commands;
public class CreateProyectoCommandHandler : IRequestHandler<CreateProyectoCommand, ProyectoDto>
{
    private readonly IProyectoRepository _proyectoRepository;
    private readonly IMapper _mapper;

    public CreateProyectoCommandHandler(IProyectoRepository proyectoRepository, IMapper mapper)
    {
        _proyectoRepository = proyectoRepository;
        _mapper = mapper;
    }

    public async Task<ProyectoDto> Handle(CreateProyectoCommand request, CancellationToken cancellationToken)
    {
        var proyecto = new Proyecto
        {
            FechaCreacion = DateTime.UtcNow,
            UsuarioId = request.UsuarioId
        };

        var createdProyecto = await _proyectoRepository.CreateAsync(proyecto);
        return _mapper.Map<ProyectoDto>(createdProyecto);
    }
}