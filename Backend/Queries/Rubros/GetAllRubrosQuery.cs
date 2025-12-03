using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.Rubros;

public class GetAllRubrosQuery : IRequest<List<RubroDto>> { }
