using AutoMapper;
using Backend.Commands.CadenasDeValor;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateCadenaDeValorCommandHandler : IRequestHandler<CreateCadenaDeValorCommand, CadenaDeValorDto>
{
    private readonly ICadenaDeValorRepository _repo;
    private readonly IMapper _mapper;
    public CreateCadenaDeValorCommandHandler(ICadenaDeValorRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<CadenaDeValorDto> Handle(CreateCadenaDeValorCommand request, CancellationToken cancellationToken)
    {
        var entity = new CadenaDeValor { Nombre = request.Nombre, ObjetivoEspecifico = request.ObjetivoEspecifico, Producto = request.Producto };
        var created = await _repo.CreateAsync(entity);
        return _mapper.Map<CadenaDeValorDto>(created);
    }
}
