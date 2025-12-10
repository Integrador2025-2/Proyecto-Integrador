using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.Proyectos;

public class GetProyectoByIdQuery : IRequest<ProyectoDto?>
{
    public int ProyectoId { get; }
    public GetProyectoByIdQuery(int proyectoId) => ProyectoId = proyectoId;
}
