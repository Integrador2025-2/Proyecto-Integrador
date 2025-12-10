namespace Backend.Models.DTOs;

public class AdministrativosDto
{
    public int AdministrativoId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string Cargo { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
}

public class CreateAdministrativosDto
{
    public int RecursoEspecificoId { get; set; }
    public string Cargo { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
}

public class UpdateAdministrativosDto
{
    public int AdministrativoId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string Cargo { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
}
