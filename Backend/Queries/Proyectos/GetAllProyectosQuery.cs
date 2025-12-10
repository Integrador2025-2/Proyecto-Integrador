using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.Proyectos;

public class GetAllProyectosQuery : IRequest<List<ProyectoDto>>
{
}
