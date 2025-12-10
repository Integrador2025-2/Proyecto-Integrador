namespace Backend.Models.Domain;

public class TalentoHumano
{
    public int TalentoHumanoId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public int ContratacionId { get; set; }
    public string CargoEspecifico { get; set; } = string.Empty;
    public int Semanas { get; set; }
    public decimal Total { get; set; }

    // Navigation properties
    public RecursoEspecifico? RecursoEspecifico { get; set; }
    public Contratacion? Contratacion { get; set; }
    public List<TalentoHumanoTarea>? TalentoHumanoTareas { get; set; }
    public List<RemuneracionPorAnio>? Remuneraciones { get; set; }
}
