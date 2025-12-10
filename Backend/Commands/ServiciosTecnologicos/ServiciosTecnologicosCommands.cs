using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.ServiciosTecnologicos;

public class CreateServiciosTecnologicosCommand : IRequest<ServiciosTecnologicosDto>
{
    public int RecursoEspecificoId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}

public class UpdateServiciosTecnologicosCommand : IRequest<ServiciosTecnologicosDto>
{
    public int ServiciosTecnologicosId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}

public class DeleteServiciosTecnologicosCommand : IRequest<bool>
{
    public int ServiciosTecnologicosId { get; set; }
}
