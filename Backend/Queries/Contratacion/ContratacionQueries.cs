using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Contratacion;

public record GetContratacionByIdQuery(int ContratacionId) : IRequest<ContratacionDto>;

public record GetAllContratacionesQuery : IRequest<IEnumerable<ContratacionDto>>;

public record GetContratacionesByCategoriaQuery(string Categoria) : IRequest<IEnumerable<ContratacionDto>>;
