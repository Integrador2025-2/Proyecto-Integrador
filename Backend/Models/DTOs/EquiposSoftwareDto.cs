namespace Backend.Models.DTOs;

public class EquiposSoftwareDto
{
    public int EquiposSoftwareId { get; set; }
    public int RubroId { get; set; }
    public string EspecificacionesTecnicas { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}

public class CreateEquiposSoftwareDto
{
    public int RubroId { get; set; }
    public string EspecificacionesTecnicas { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}

public class UpdateEquiposSoftwareDto
{
    public int EquiposSoftwareId { get; set; }
    public int RubroId { get; set; }
    public string EspecificacionesTecnicas { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}



