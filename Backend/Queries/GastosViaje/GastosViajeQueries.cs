using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.GastosViaje;

public class GetGastosViajeByIdQuery : IRequest<GastosViajeDto?>
{
    public int GastosViajeId { get; set; }
}

public class GetAllGastosViajeQuery : IRequest<IEnumerable<GastosViajeDto>>
{
}

public class GetGastosViajeByRecursoEspecificoIdQuery : IRequest<GastosViajeDto?>
{
    public int RecursoEspecificoId { get; set; }
}
