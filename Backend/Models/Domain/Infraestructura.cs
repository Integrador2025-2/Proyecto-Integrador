using System.ComponentModel.DataAnnotations;
namespace Backend.Models.Domain;

public class Infraestructura
{
    [Key]
    public int InfraestructuraId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string TipoInfraestructura { get; set; } = string.Empty;
    public string Enlace { get; set; } = string.Empty;
    public string CaracteristicasTecnicas { get; set; } = string.Empty;

    // Navigation properties
    public RecursoEspecifico? RecursoEspecifico { get; set; }
}
