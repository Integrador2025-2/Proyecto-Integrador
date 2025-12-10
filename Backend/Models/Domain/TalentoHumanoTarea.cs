using System.ComponentModel.DataAnnotations;
namespace Backend.Models.Domain;

public class TalentoHumanoTarea
{
    [Key]
    public int TalentoHumanoTareasId { get; set; }
    public int TalentoHumanoId { get; set; }
    public int Tarea { get; set; }
    public int HorasAsignadas { get; set; }
    public string RolenTarea { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
    public DateTime FechaAsignacion { get; set; }

    // Navigation properties
    public TalentoHumano? TalentoHumano { get; set; }
    public Tarea? TareaNavigation { get; set; }
}
