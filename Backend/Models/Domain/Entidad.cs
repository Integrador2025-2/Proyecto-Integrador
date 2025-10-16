namespace Backend.Models.Domain;

public class Entidad
{
    public int EntidadId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    
    // Actividades en las que la entidad ha participado
    public List<ActXEntidad>? ActXEntidades { get; set; }
}