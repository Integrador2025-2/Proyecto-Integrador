using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.ProteccionConocimientoDivulgacion;

public class GetProteccionConocimientoDivulgacionByIdQuery : IRequest<ProteccionConocimientoDivulgacionDto>
{
    public int ProteccionId { get; set; }
}

public class GetAllProteccionConocimientoDivulgacionQuery : IRequest<IEnumerable<ProteccionConocimientoDivulgacionDto>>
{
}

public class GetProteccionConocimientoDivulgacionByRecursoEspecificoIdQuery : IRequest<ProteccionConocimientoDivulgacionDto?>
{
    public int RecursoEspecificoId { get; set; }
}
