using System.ComponentModel.DataAnnotations;
namespace Backend.Models.Domain;

public class Objetivo
{
    [Key]
    public int ObjetivoId { get; set; }
    public int ProyectoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string ResultadoEsperado { get; set; } = string.Empty;

    // Navigation properties
    public Proyecto? Proyecto { get; set; }
    public List<CadenaDeValor>? CadenasDeValor { get; set; }
}
