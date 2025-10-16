using AutoMapper;
using Backend.Commands.CadenasDeValor;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class UpdateCadenaDeValorCommandHandler : IRequestHandler<UpdateCadenaDeValorCommand, CadenaDeValorDto>
{
    private readonly ICadenaDeValorRepository _repo;
    private readonly IMapper _mapper;
    public UpdateCadenaDeValorCommandHandler(ICadenaDeValorRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<CadenaDeValorDto> Handle(UpdateCadenaDeValorCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repo.GetByIdAsync(request.CadenaDeValorId) ?? throw new KeyNotFoundException("CadenaDeValor not found");
        existing.Nombre = request.Nombre;
        existing.ObjetivoEspecifico = request.ObjetivoEspecifico;
        existing.Producto = request.Producto;
        var updated = await _repo.UpdateAsync(existing);
        return _mapper.Map<CadenaDeValorDto>(updated);
    }
}
