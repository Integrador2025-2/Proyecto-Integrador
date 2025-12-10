using System.ComponentModel.DataAnnotations;
namespace Backend.Models.Domain;

public class SeguimientoEvaluacion
{
    [Key]
    public int SeguimientoId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string CargoResponsable { get; set; } = string.Empty;
    public string MetodoEvaluacion { get; set; } = string.Empty;
    public string Frecuencia { get; set; } = string.Empty;

    // Navigation properties
    public RecursoEspecifico? RecursoEspecifico { get; set; }
}
