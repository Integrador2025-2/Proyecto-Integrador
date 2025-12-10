using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.CadenasDeValor;

public class GetCadenaDeValorByIdQuery : IRequest<CadenaDeValorDto?>
{
    public int CadenaDeValorId { get; set; }
}

public class GetAllCadenasDeValorQuery : IRequest<IEnumerable<CadenaDeValorDto>>
{
}

public class GetCadenasDeValorByObjetivoIdQuery : IRequest<IEnumerable<CadenaDeValorDto>>
{
    public int ObjetivoId { get; set; }
}
