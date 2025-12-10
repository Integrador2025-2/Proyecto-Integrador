namespace Backend.Models.DTOs;

public class CapacitacionEventosDto
{
    public int CapacitacionEventosId { get; set; }
    public int RubroId { get; set; }
    public string Tema { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}
public class CreateCapacitacionEventosDto
{
    public int RubroId { get; set; }
    public string Tema { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}
public class UpdateCapacitacionEventosDto
{
    public int CapacitacionEventosId { get; set; }
    public int RubroId { get; set; }
    public string Tema { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}
