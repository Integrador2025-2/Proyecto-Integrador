using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.CapacitacionEventos;

public class GetAllCapacitacionEventosQuery : IRequest<List<CapacitacionEventosDto>> { }
