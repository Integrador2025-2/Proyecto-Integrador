namespace Backend.Models.DTOs;

public class ServiciosTecnologicosDto
{
    public int ServiciosTecnologicosId { get; set; }
    public int RubroId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}

public class CreateServiciosTecnologicosDto
{
    public int RubroId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}
public class UpdateServiciosTecnologicosDto
{
    public int ServiciosTecnologicosId { get; set; }
    public int RubroId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string RagEstado { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}

