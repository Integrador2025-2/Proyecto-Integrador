namespace Backend.Models.Domain;

public class ActXEntidad
{
    public int Id { get; set; }
    public int ActividadId { get; set; }
    public int EntidadId { get; set; }
    public decimal Efectivo { get; set; }
    public decimal Especie { get; set; }
    public decimal TotalContribucion { get; set; }
    
    // Navigation properties
    public Actividad? Actividad { get; set; }
    public Entidad? Entidad { get; set; }
}