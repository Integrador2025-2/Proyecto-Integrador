namespace Backend.Models.DTOs;

public class TalentoHumanoTareaDto
{
    public int TalentoHumanoTareasId { get; set; }
    public int TalentoHumanoId { get; set; }
    public int Tarea { get; set; }
    public int HorasAsignadas { get; set; }
    public string RolenTarea { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
    public DateTime FechaAsignacion { get; set; }
}
