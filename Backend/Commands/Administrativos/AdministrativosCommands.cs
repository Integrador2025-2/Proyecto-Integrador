using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.Administrativos;

public class CreateAdministrativosCommand : IRequest<AdministrativosDto>
{
    public int RecursoEspecificoId { get; set; }
    public string Cargo { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
}

public class UpdateAdministrativosCommand : IRequest<AdministrativosDto>
{
    public int AdministrativoId { get; set; }
    public string Cargo { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
}

public class DeleteAdministrativosCommand : IRequest<bool>
{
    public int AdministrativoId { get; set; }
}
