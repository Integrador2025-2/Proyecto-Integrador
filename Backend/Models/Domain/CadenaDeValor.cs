namespace Backend.Models.Domain;

public class CadenaDeValor
{
    public int CadenaDeValorId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string ObjetivoEspecifico { get; set; } = string.Empty;
    public string Producto { get; set; } = string.Empty;

    // Actividades asociadas a esta cadena de valor
    public List<Actividad>? Actividades { get; set; }
}