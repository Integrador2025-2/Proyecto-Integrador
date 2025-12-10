namespace Backend.Models.DTOs;

public class ProteccionConocimientoDivulgacionDto
{
    public int ProteccionId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string ActividadHapat { get; set; } = string.Empty;
    public string EntidadResponsable { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
}
