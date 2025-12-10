using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.Actividades;

public class GetActividadesByProyectoQuery : IRequest<List<ActividadDto>>
{
    public int ProyectoId { get; }
    public GetActividadesByProyectoQuery(int proyectoId) => ProyectoId = proyectoId;
}
