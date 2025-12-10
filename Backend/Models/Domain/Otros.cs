namespace Backend.Models.Domain;

public class Otros
{
    public int OtrosId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string Justificacion { get; set; } = string.Empty;

    // Navigation properties
    public RecursoEspecifico? RecursoEspecifico { get; set; }
}
