using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetAllCadenasDeValorQueryHandler : IRequestHandler<Backend.Queries.CadenasDeValor.GetAllCadenasDeValorQuery, List<CadenaDeValorDto>>
{
    private readonly ICadenaDeValorRepository _repo;
    private readonly IMapper _mapper;
    public GetAllCadenasDeValorQueryHandler(ICadenaDeValorRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<List<CadenaDeValorDto>> Handle(Backend.Queries.CadenasDeValor.GetAllCadenasDeValorQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetAllAsync();
        return _mapper.Map<List<CadenaDeValorDto>>(items);
    }
}
