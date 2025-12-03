namespace Backend.Models.DTOs;

public class TalentoHumanoDto
{
    public int TalentoHumanoId { get; set; }
    public int RubroId { get; set; }
    public string CargoEspecifico { get; set; } = string.Empty;
    public int Semanas { get; set; }
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}

public class CreateTalentoHumanoDto
{
    public int RubroId { get; set; }
    public string CargoEspecifico { get; set; } = string.Empty;
    public int Semanas { get; set; }
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}
public class UpdateTalentoHumanoDto
{
    public int TalentoHumanoId { get; set; }
    public int RubroId { get; set; }
    public string CargoEspecifico { get; set; } = string.Empty;
    public int Semanas { get; set; }
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}

