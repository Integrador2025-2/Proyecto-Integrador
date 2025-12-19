namespace Backend.Models.Domain;

public class EquiposSoftware
{
    public int EquiposSoftwareId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string EspecificacionesTecnicas { get; set; } = string.Empty;

    // Navigation properties
    public RecursoEspecifico? RecursoEspecifico { get; set; }
}
