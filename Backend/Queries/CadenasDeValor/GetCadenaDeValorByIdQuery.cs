using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.CadenasDeValor;

public class GetCadenaDeValorByIdQuery : IRequest<CadenaDeValorDto?>
{
    public int CadenaDeValorId { get; set; }
    public GetCadenaDeValorByIdQuery(int id) => CadenaDeValorId = id;
}
