namespace Backend.Models.Domain;

public class Tarea
{
    public int TareaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Periodo { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    // Relación con Actividad
    public int ActividadId { get; set; }
    public Actividad Actividad { get; set; } = null!;
}