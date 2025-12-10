using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.CadenasDeValor;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetCadenaDeValorByIdHandler : IRequestHandler<GetCadenaDeValorByIdQuery, CadenaDeValorDto?>
{
    private readonly ICadenaDeValorRepository _cadenaDeValorRepository;

    public GetCadenaDeValorByIdHandler(ICadenaDeValorRepository cadenaDeValorRepository)
    {
        _cadenaDeValorRepository = cadenaDeValorRepository;
    }

    public async Task<CadenaDeValorDto?> Handle(GetCadenaDeValorByIdQuery request, CancellationToken cancellationToken)
    {
        var cadenaDeValor = await _cadenaDeValorRepository.GetByIdAsync(request.CadenaDeValorId);
        if (cadenaDeValor == null)
            return null;

        return new CadenaDeValorDto
        {
            CadenaDeValorId = cadenaDeValor.CadenaDeValorId,
            ObjetivoId = cadenaDeValor.ObjetivoId,
            Nombre = cadenaDeValor.Nombre,
            ObjetivoEspecifico = cadenaDeValor.ObjetivoEspecifico,
            ObjetivoNombre = cadenaDeValor.Objetivo?.Nombre
        };
    }
}

public class GetAllCadenasDeValorHandler : IRequestHandler<GetAllCadenasDeValorQuery, IEnumerable<CadenaDeValorDto>>
{
    private readonly ICadenaDeValorRepository _cadenaDeValorRepository;

    public GetAllCadenasDeValorHandler(ICadenaDeValorRepository cadenaDeValorRepository)
    {
        _cadenaDeValorRepository = cadenaDeValorRepository;
    }

    public async Task<IEnumerable<CadenaDeValorDto>> Handle(GetAllCadenasDeValorQuery request, CancellationToken cancellationToken)
    {
        var cadenasDeValor = await _cadenaDeValorRepository.GetAllAsync();

        return cadenasDeValor.Select(c => new CadenaDeValorDto
        {
            CadenaDeValorId = c.CadenaDeValorId,
            ObjetivoId = c.ObjetivoId,
            Nombre = c.Nombre,
            ObjetivoEspecifico = c.ObjetivoEspecifico,
            ObjetivoNombre = c.Objetivo?.Nombre
        });
    }
}

public class GetCadenasDeValorByObjetivoIdHandler : IRequestHandler<GetCadenasDeValorByObjetivoIdQuery, IEnumerable<CadenaDeValorDto>>
{
    private readonly ICadenaDeValorRepository _cadenaDeValorRepository;

    public GetCadenasDeValorByObjetivoIdHandler(ICadenaDeValorRepository cadenaDeValorRepository)
    {
        _cadenaDeValorRepository = cadenaDeValorRepository;
    }

    public async Task<IEnumerable<CadenaDeValorDto>> Handle(GetCadenasDeValorByObjetivoIdQuery request, CancellationToken cancellationToken)
    {
        var cadenasDeValor = await _cadenaDeValorRepository.GetByObjetivoIdAsync(request.ObjetivoId);

        return cadenasDeValor.Select(c => new CadenaDeValorDto
        {
            CadenaDeValorId = c.CadenaDeValorId,
            ObjetivoId = c.ObjetivoId,
            Nombre = c.Nombre,
            ObjetivoEspecifico = c.ObjetivoEspecifico,
            ObjetivoNombre = c.Objetivo?.Nombre
        });
    }
}
