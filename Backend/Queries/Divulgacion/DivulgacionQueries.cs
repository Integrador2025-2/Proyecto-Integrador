using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Divulgacion;

public class GetDivulgacionByIdQuery : IRequest<DivulgacionDto>
{
    public int DivulgacionId { get; set; }
}

public class GetAllDivulgacionQuery : IRequest<IEnumerable<DivulgacionDto>>
{
}

public class GetDivulgacionByRecursoEspecificoIdQuery : IRequest<DivulgacionDto?>
{
    public int RecursoEspecificoId { get; set; }
}
