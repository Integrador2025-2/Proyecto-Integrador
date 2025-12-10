using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.ActxEntidad;

public class GetActXEntidadByIdQuery : IRequest<ActXEntidadDto?>
{
    public int Id { get; set; }
}

public class GetAllActXEntidadesQuery : IRequest<IEnumerable<ActXEntidadDto>>
{
}

public class GetActXEntidadesByActividadIdQuery : IRequest<IEnumerable<ActXEntidadDto>>
{
    public int ActividadId { get; set; }
}

public class GetActXEntidadesByEntidadIdQuery : IRequest<IEnumerable<ActXEntidadDto>>
{
    public int EntidadId { get; set; }
}
