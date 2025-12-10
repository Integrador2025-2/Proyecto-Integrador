namespace Backend.Models.Domain;

public class MaterialesInsumos
{
    public int MaterialesInsumosId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string Materiales { get; set; } = string.Empty;

    // Navigation properties
    public RecursoEspecifico? RecursoEspecifico { get; set; }
}
