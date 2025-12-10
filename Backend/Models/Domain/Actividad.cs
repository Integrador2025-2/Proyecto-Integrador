namespace Backend.Models.Domain;

public class Actividad
{
    public int ActividadId { get; set; }
    public int CadenaDeValorId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
    public int DuracionAnios { get; set; }
    public string EspecificacionesTecnicas { get; set; } = string.Empty;
    public decimal ValorUnitario { get; set; }

    // Navigation properties
    public CadenaDeValor? CadenaDeValor { get; set; }
    public List<Tarea>? Tareas { get; set; }
    public List<ActXEntidad>? ActXEntidades { get; set; }
    public List<Recurso>? Recursos { get; set; }
}
