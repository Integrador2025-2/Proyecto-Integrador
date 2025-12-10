namespace Backend.Models.Domain;

public class GastosViaje
{
    public int GastosViajeId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public decimal Costo { get; set; }

    // Navigation properties
    public RecursoEspecifico? RecursoEspecifico { get; set; }
}
