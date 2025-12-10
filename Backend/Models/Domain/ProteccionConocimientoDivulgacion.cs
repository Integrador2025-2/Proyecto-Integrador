using System.ComponentModel.DataAnnotations;
namespace Backend.Models.Domain;

public class ProteccionConocimientoDivulgacion
{
    [Key]
    public int ProteccionId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string ActividadHapat { get; set; } = string.Empty;
    public string EntidadResponsable { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;

    // Navigation properties
    public RecursoEspecifico? RecursoEspecifico { get; set; }
}
