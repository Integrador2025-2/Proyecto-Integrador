using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.CadenasDeValor;

public class GetAllCadenasDeValorQuery : IRequest<List<CadenaDeValorDto>> { }
