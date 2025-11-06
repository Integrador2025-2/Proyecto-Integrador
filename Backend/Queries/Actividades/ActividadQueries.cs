using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Actividades;

public class GetActividadByIdQuery : IRequest<ActividadDto?>
{
    public int ActividadId { get; set; }
}

public class GetAllActividadesQuery : IRequest<IEnumerable<ActividadDto>>
{
}

public class GetActividadesByCadenaDeValorIdQuery : IRequest<IEnumerable<ActividadDto>>
{
    public int CadenaDeValorId { get; set; }
}
