namespace Backend.Models.DTOs;

public class SeguimientoEvaluacionDto
{
    public int SeguimientoId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string CargoResponsable { get; set; } = string.Empty;
    public string MetodoEvaluacion { get; set; } = string.Empty;
    public string Frecuencia { get; set; } = string.Empty;
}
