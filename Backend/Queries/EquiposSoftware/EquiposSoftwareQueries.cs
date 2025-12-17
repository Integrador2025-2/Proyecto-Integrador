using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.EquiposSoftware;

public class GetEquiposSoftwareByIdQuery : IRequest<EquiposSoftwareDto>
{
    public int EquiposSoftwareId { get; set; }
}

public class GetAllEquiposSoftwareQuery : IRequest<IEnumerable<EquiposSoftwareDto>>
{
}

public class GetEquiposSoftwareByRecursoEspecificoIdQuery : IRequest<EquiposSoftwareDto?>
{
    public int RecursoEspecificoId { get; set; }
}
