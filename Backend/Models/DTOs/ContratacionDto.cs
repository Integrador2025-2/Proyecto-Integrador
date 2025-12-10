namespace Backend.Models.DTOs;

public class ContratacionDto
{
    public int ContratacionId { get; set; }
    public string NivelGestion { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string IdentidadAcademica { get; set; } = string.Empty;
    public string ExperienciaMinima { get; set; } = string.Empty;
    public decimal Iva { get; set; }
    public decimal ValorMensual { get; set; }
}

public class CreateContratacionDto
{
    public string NivelGestion { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string IdentidadAcademica { get; set; } = string.Empty;
    public string ExperienciaMinima { get; set; } = string.Empty;
    public decimal Iva { get; set; }
    public decimal ValorMensual { get; set; }
}

public class UpdateContratacionDto
{
    public int ContratacionId { get; set; }
    public string NivelGestion { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string IdentidadAcademica { get; set; } = string.Empty;
    public string ExperienciaMinima { get; set; } = string.Empty;
    public decimal Iva { get; set; }
    public decimal ValorMensual { get; set; }
}
