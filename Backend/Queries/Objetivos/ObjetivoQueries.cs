using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Objetivos;

public class GetObjetivoByIdQuery : IRequest<ObjetivoDto?>
{
    public int ObjetivoId { get; set; }
}

public class GetAllObjetivosQuery : IRequest<IEnumerable<ObjetivoDto>>
{
}

public class GetObjetivosByProyectoIdQuery : IRequest<IEnumerable<ObjetivoDto>>
{
    public int ProyectoId { get; set; }
}
