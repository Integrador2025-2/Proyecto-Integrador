using Backend.Commands.CadenasDeValor;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateCadenaDeValorHandler : IRequestHandler<CreateCadenaDeValorCommand, CadenaDeValorDto>
{
    private readonly ICadenaDeValorRepository _cadenaDeValorRepository;

    public CreateCadenaDeValorHandler(ICadenaDeValorRepository cadenaDeValorRepository)
    {
        _cadenaDeValorRepository = cadenaDeValorRepository;
    }

    public async Task<CadenaDeValorDto> Handle(CreateCadenaDeValorCommand request, CancellationToken cancellationToken)
    {
        var cadenaDeValor = new CadenaDeValor
        {
            ObjetivoId = request.ObjetivoId,
            Nombre = request.Nombre,
            ObjetivoEspecifico = request.ObjetivoEspecifico
        };

        var createdCadenaDeValor = await _cadenaDeValorRepository.CreateAsync(cadenaDeValor);

        return new CadenaDeValorDto
        {
            CadenaDeValorId = createdCadenaDeValor.CadenaDeValorId,
            ObjetivoId = createdCadenaDeValor.ObjetivoId,
            Nombre = createdCadenaDeValor.Nombre,
            ObjetivoEspecifico = createdCadenaDeValor.ObjetivoEspecifico
        };
    }
}

public class UpdateCadenaDeValorHandler : IRequestHandler<UpdateCadenaDeValorCommand, CadenaDeValorDto>
{
    private readonly ICadenaDeValorRepository _cadenaDeValorRepository;

    public UpdateCadenaDeValorHandler(ICadenaDeValorRepository cadenaDeValorRepository)
    {
        _cadenaDeValorRepository = cadenaDeValorRepository;
    }

    public async Task<CadenaDeValorDto> Handle(UpdateCadenaDeValorCommand request, CancellationToken cancellationToken)
    {
        var cadenaDeValor = await _cadenaDeValorRepository.GetByIdAsync(request.CadenaDeValorId);
        if (cadenaDeValor == null)
            throw new KeyNotFoundException($"CadenaDeValor with ID {request.CadenaDeValorId} not found");

        cadenaDeValor.Nombre = request.Nombre;
        cadenaDeValor.ObjetivoEspecifico = request.ObjetivoEspecifico;

        var updatedCadenaDeValor = await _cadenaDeValorRepository.UpdateAsync(cadenaDeValor);

        return new CadenaDeValorDto
        {
            CadenaDeValorId = updatedCadenaDeValor.CadenaDeValorId,
            ObjetivoId = updatedCadenaDeValor.ObjetivoId,
            Nombre = updatedCadenaDeValor.Nombre,
            ObjetivoEspecifico = updatedCadenaDeValor.ObjetivoEspecifico
        };
    }
}

public class DeleteCadenaDeValorHandler : IRequestHandler<DeleteCadenaDeValorCommand, bool>
{
    private readonly ICadenaDeValorRepository _cadenaDeValorRepository;

    public DeleteCadenaDeValorHandler(ICadenaDeValorRepository cadenaDeValorRepository)
    {
        _cadenaDeValorRepository = cadenaDeValorRepository;
    }

    public async Task<bool> Handle(DeleteCadenaDeValorCommand request, CancellationToken cancellationToken)
    {
        var exists = await _cadenaDeValorRepository.ExistsAsync(request.CadenaDeValorId);
        if (!exists)
            return false;

        await _cadenaDeValorRepository.DeleteAsync(request.CadenaDeValorId);
        return true;
    }
}
