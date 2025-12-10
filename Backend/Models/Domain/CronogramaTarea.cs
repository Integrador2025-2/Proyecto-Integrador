using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Domain;

public class CronogramaTarea
{
    [Key]
    public int CronogramaId { get; set; }
    public int TareaId { get; set; }
    public int DuracionMeses { get; set; }
    public int DuracionDias { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }

    // Navigation properties
    public Tarea? Tarea { get; set; }
}
