namespace Backend.Models.Domain;

public class CadenaDeValor
{
    public int CadenaDeValorId { get; set; }
    public int ObjetivoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string ObjetivoEspecifico { get; set; } = string.Empty;

    // Navigation properties
    public Objetivo? Objetivo { get; set; }
    public List<Actividad>? Actividades { get; set; }
}