using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Administrativos;

public class GetAdministrativosByIdQuery : IRequest<AdministrativosDto>
{
    public int AdministrativoId { get; set; }
}

public class GetAllAdministrativosQuery : IRequest<IEnumerable<AdministrativosDto>>
{
}

public class GetAdministrativosByRecursoEspecificoIdQuery : IRequest<AdministrativosDto?>
{
    public int RecursoEspecificoId { get; set; }
}
