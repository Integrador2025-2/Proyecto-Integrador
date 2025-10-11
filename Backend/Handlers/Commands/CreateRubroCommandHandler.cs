using AutoMapper;
using Backend.Commands.Rubros;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateRubroCommandHandler : IRequestHandler<CreateRubroCommand, RubroDto>
{
    private readonly IRubroRepository _rubroRepository;
    private readonly IMapper _mapper;
    public CreateRubroCommandHandler(IRubroRepository rubroRepository, IMapper mapper)
    {
        _rubroRepository = rubroRepository; _mapper = mapper;
    }

    public async Task<RubroDto> Handle(CreateRubroCommand request, CancellationToken cancellationToken)
    {
        var rubro = new Rubro { Descripcion = request.Descripcion };
        var created = await _rubroRepository.CreateAsync(rubro);
        return _mapper.Map<RubroDto>(created);
    }
}
