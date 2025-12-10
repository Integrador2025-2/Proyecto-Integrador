using System.ComponentModel.DataAnnotations;
namespace Backend.Models.Domain;

public class RemuneracionPorAnio
{
    [Key]
    public int RemuneracionPorAnioId { get; set; }
    public int TalentoHumanoId { get; set; }
    public int Anio { get; set; }
    public decimal Honorarios { get; set; }
    public decimal ValorHora { get; set; }
    public int SemanasAnio { get; set; }
    public decimal TotalAnio { get; set; }

    // Navigation properties
    public TalentoHumano? TalentoHumano { get; set; }
}
