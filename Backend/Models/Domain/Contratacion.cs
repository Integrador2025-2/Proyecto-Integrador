using System.ComponentModel.DataAnnotations;
namespace Backend.Models.Domain;

public class Contratacion
{
    [Key]
    public int ContratacionId { get; set; }
    public string NivelGestion { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string IdentidadAcademica { get; set; } = string.Empty;
    public string ExperienciaMinima { get; set; } = string.Empty;
    public decimal Iva { get; set; }
    public decimal ValorMensual { get; set; }

    // Navigation properties
    public List<TalentoHumano>? TalentosHumanos { get; set; }
}
