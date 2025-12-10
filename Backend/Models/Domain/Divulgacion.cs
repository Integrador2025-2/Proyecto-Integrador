namespace Backend.Models.Domain;

public class Divulgacion
{
    public int DivulgacionId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string MedioDivulgacion { get; set; } = string.Empty;
    public string Alcance { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;

    // Navigation properties
    public RecursoEspecifico? RecursoEspecifico { get; set; }
}
