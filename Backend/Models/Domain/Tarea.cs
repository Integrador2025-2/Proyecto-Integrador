namespace Backend.Models.Domain;

public class Tarea
{
    public int TareaId { get; set; }
    public int ActividadId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Periodo { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    
    // Navigation properties
    public Actividad? Actividad { get; set; }
    public List<CronogramaTarea>? CronogramaTareas { get; set; }
    public List<TalentoHumanoTarea>? TalentoHumanoTareas { get; set; }
}