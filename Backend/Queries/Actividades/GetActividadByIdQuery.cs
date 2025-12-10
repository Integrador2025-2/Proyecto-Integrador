using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.Actividades;

public class GetActividadByIdQuery : IRequest<ActividadDto?>
{
    public int ActividadId { get; }
    public GetActividadByIdQuery(int actividadId) => ActividadId = actividadId;
}
