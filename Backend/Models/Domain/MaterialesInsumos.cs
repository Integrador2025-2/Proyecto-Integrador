namespace Backend.Models.Domain;

public class MaterialesInsumos
{
    public int MaterialesInsumosId { get; set; }
    public int RubroId { get; set; }
    public string Materiales { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;

    // Optional navigation back to Actividad
    public int? ActividadId { get; set; }
    public Actividad? Actividad { get; set; }
}
