using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Rubros;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetRubroByIdQueryHandler : IRequestHandler<GetRubroByIdQuery, RubroDto?>
{
    private readonly IRubroRepository _rubroRepository;
    private readonly IMapper _mapper;
    public GetRubroByIdQueryHandler(IRubroRepository rubroRepository, IMapper mapper)
    {
        _rubroRepository = rubroRepository; _mapper = mapper;
    }

    public async Task<RubroDto?> Handle(GetRubroByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _rubroRepository.GetByIdAsync(request.RubroId);
        return item == null ? null : _mapper.Map<RubroDto>(item);
    }
}
