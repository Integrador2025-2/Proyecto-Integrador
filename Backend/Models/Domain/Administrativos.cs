using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Domain;

public class Administrativos
{
    [Key]
    public int AdministrativoId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string Cargo { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;

    // Navigation properties
    public RecursoEspecifico? RecursoEspecifico { get; set; }
}
