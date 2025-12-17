namespace Backend.Models.Domain;

public class ServiciosTecnologicos
{
    public int ServiciosTecnologicosId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string Descripcion { get; set; } = string.Empty;

    // Navigation properties
    public RecursoEspecifico? RecursoEspecifico { get; set; }
}
