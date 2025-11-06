namespace Backend.Models.DTOs;

public class ActividadDto
{
    public int ActividadId { get; set; }
    public int CadenaDeValorId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
    public int DuracionAnios { get; set; }
    public string EspecificacionesTecnicas { get; set; } = string.Empty;
    public decimal ValorUnitario { get; set; }
    public string? CadenaDeValorNombre { get; set; }
}

public class CreateActividadDto
{
    public int CadenaDeValorId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
    public int DuracionAnios { get; set; }
    public string EspecificacionesTecnicas { get; set; } = string.Empty;
    public decimal ValorUnitario { get; set; }
}

public class UpdateActividadDto
{
    public int ActividadId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
    public int DuracionAnios { get; set; }
    public string EspecificacionesTecnicas { get; set; } = string.Empty;
    public decimal ValorUnitario { get; set; }
}
