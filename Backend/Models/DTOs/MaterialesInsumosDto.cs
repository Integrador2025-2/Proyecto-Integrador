namespace Backend.Models.DTOs;

public class MaterialesInsumosDto
{
    public int MaterialesInsumosId { get; set; }
    public int RubroId { get; set; }
    public string Materiales { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}

public class CreateMaterialesInsumosDto
{
    public int RubroId { get; set; }
    public string Materiales { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}
public class UpdateMaterialesInsumosDto
{
    public int MaterialesInsumosId { get; set; }
    public int RubroId { get; set; }
    public string Materiales { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}


