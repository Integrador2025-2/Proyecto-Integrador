namespace Backend.Models.DTOs;

public class GastosViajeDto
{
    public int GastosViajeId { get; set; }
    public int RubroId { get; set; }
    public decimal Costo { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}

public class CreateGastosViajeDto
{
    public int RubroId { get; set; }
    public decimal Costo { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}
public class UpdateGastosViajeDto
{
    public int GastosViajeId { get; set; }
    public int RubroId { get; set; }
    public decimal Costo { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}
