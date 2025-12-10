using AutoMapper;
using Backend.Commands.Rubros;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class UpdateRubroCommandHandler : IRequestHandler<UpdateRubroCommand, RubroDto?>
{
    private readonly IRubroRepository _rubroRepository;
    private readonly IMapper _mapper;
    public UpdateRubroCommandHandler(IRubroRepository rubroRepository, IMapper mapper)
    {
        _rubroRepository = rubroRepository; _mapper = mapper;
    }

    public async Task<RubroDto?> Handle(UpdateRubroCommand request, CancellationToken cancellationToken)
    {
        var rubro = new Rubro { RubroId = request.RubroId, Descripcion = request.Descripcion };
        var updated = await _rubroRepository.UpdateAsync(rubro);
        return updated == null ? null : _mapper.Map<RubroDto>(updated);
    }
}
