namespace Backend.Models.DTOs;

public class ActividadDto
{
    public int ActividadId { get; set; }
    public int ProyectoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
    public List<decimal> TotalxAnios { get; set; } = new List<decimal>();
    public int CantidadAnios { get; set; }
    public string EspecificacionesTecnicas { get; set; } = string.Empty;
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }

    // Unified list of rubros as requested by the JSON structure
    public List<RubroItemDto> Rubros { get; set; } = new List<RubroItemDto>();
}

public class CreateActividadDto
{
    public int ProyectoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
    public List<decimal> TotalxAnios { get; set; } = new List<decimal>();
    public int CantidadAnios { get; set; }
    public string EspecificacionesTecnicas { get; set; } = string.Empty;
    public decimal ValorUnitario { get; set; }
}

public class UpdateActividadDto
{
    public int ActividadId { get; set; }
    public int ProyectoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
    public List<decimal> TotalxAnios { get; set; } = new List<decimal>();
    public int CantidadAnios { get; set; }
    public string EspecificacionesTecnicas { get; set; } = string.Empty;
    public decimal ValorUnitario { get; set; }
}

public class RubroItemDto
{
    public string Tipo { get; set; } = string.Empty;
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
    public int PeriodoNum { get; set; }

    // Optional extra fields depending on tipo
    public string? CargoEspecifico { get; set; }
    public int? Semanas { get; set; }
    public int? Cantidad { get; set; }
}

