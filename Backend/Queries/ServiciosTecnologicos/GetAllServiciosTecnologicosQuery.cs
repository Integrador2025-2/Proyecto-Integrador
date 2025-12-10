using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.ServiciosTecnologicos;

public class GetAllServiciosTecnologicosQuery : IRequest<List<ServiciosTecnologicosDto>> { }
