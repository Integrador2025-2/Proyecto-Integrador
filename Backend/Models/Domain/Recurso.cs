using System.ComponentModel.DataAnnotations;
namespace Backend.Models.Domain;

public class Recurso
{
    [Key]
    public int RecursoId { get; set; }
    public int ActividadId { get; set; }
    public int EntidadId { get; set; }
    public int RubroId { get; set; }
    public string TipoRecurso { get; set; } = string.Empty;
    public decimal MontoEfectivo { get; set; }
    public decimal MontoEspecie { get; set; }
    public string Descripcion { get; set; } = string.Empty;

    // Navigation properties
    public Actividad? Actividad { get; set; }
    public Entidad? Entidad { get; set; }
    public Rubro? Rubro { get; set; }
    public List<RecursoEspecifico>? RecursosEspecificos { get; set; }
}
