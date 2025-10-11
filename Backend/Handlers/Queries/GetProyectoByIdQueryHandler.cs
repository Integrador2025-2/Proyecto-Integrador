using MediatR;
using Backend.Models.DTOs;
using Backend.Queries.Proyectos;
using Backend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Backend.Handlers.Queries
{
    public class GetProyectoByIdQueryHandler : IRequestHandler<GetProyectoByIdQuery, ProyectoDto?>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetProyectoByIdQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context; _mapper = mapper;
        }
        public async Task<ProyectoDto?> Handle(GetProyectoByIdQuery request, CancellationToken cancellationToken)
        {
            var proyecto = await _context.Proyectos
                .FirstOrDefaultAsync(p => p.ProyectoId == request.ProyectoId, cancellationToken);
            return proyecto == null ? null : _mapper.Map<ProyectoDto>(proyecto);
        }
    }
}