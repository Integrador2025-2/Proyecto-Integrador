namespace Backend.Models.Domain;

public class Proyecto
{
    public int ProyectoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public string Estado { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
    
    // Navigation properties
    public User? Usuario { get; set; }
    public List<Objetivo>? Objetivos { get; set; }
}
