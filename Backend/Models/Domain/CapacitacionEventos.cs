namespace Backend.Models.Domain;

public class CapacitacionEventos
{
    public int CapacitacionEventosId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string Tema { get; set; } = string.Empty;
    public int Cantidad { get; set; }

    // Navigation properties
    public RecursoEspecifico? RecursoEspecifico { get; set; }
}