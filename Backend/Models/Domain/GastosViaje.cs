namespace Backend.Models.Domain;

public class GastosViaje
{
    public int GastosViajeId { get; set; }
    public int RubroId { get; set; }
    public decimal Costo { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;

    public int? ActividadId { get; set; }
    public Actividad? Actividad { get; set; }
}
