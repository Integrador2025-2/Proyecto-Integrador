namespace Backend.Models.Domain;

public class TalentoHumano
{
    public int TalentoHumanoId { get; set; }
    public int RubroId { get; set; }
    public string CargoEspecifico { get; set; } = string.Empty;
    public int Semanas { get; set; }
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;

    // Optional navigation to Actividad if a TalentoHumano belongs to a specific Actividad
    public int? ActividadId { get; set; }
    public Actividad? Actividad { get; set; }
}
