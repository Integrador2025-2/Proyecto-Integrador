namespace Backend.Models.DTOs;

public class ActXEntidadDto
{
    public int Id { get; set; }
    public int ActividadId { get; set; }
    public int EntidadId { get; set; }
    public decimal Efectivo { get; set; }
    public decimal Especie { get; set; }
    public decimal TotalContribucion { get; set; }
    public string? ActividadNombre { get; set; }
    public string? EntidadNombre { get; set; }
}

public class CreateActXEntidadDto
{
    public int ActividadId { get; set; }
    public int EntidadId { get; set; }
    public decimal Efectivo { get; set; }
    public decimal Especie { get; set; }
    public decimal TotalContribucion { get; set; }
}

public class UpdateActXEntidadDto
{
    public int Id { get; set; }
    public decimal Efectivo { get; set; }
    public decimal Especie { get; set; }
    public decimal TotalContribucion { get; set; }
}
