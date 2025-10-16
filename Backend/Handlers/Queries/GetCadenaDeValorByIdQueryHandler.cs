using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetCadenaDeValorByIdQueryHandler : IRequestHandler<Backend.Queries.CadenasDeValor.GetCadenaDeValorByIdQuery, CadenaDeValorDto?>
{
    private readonly ICadenaDeValorRepository _repo;
    private readonly IMapper _mapper;
    public GetCadenaDeValorByIdQueryHandler(ICadenaDeValorRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<CadenaDeValorDto?> Handle(Backend.Queries.CadenasDeValor.GetCadenaDeValorByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.CadenaDeValorId);
        return item == null ? null : _mapper.Map<CadenaDeValorDto>(item);
    }
}
