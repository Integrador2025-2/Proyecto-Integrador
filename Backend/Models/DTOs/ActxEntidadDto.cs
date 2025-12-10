namespace Backend.Models.DTOs;

public class ActxEntidadDto
{
    public int ActXEntidadId { get; set; }
    public int ActividadId { get; set; }
    public int EntidadId { get; set; }
    public decimal Efectivo { get; set; }
    public decimal Especie { get; set; }

    // Optional nested entity info
    public EntidadDto? Entidad { get; set; }
}

public class CreateActxEntidadDto
{
    public int ActividadId { get; set; }
    public int EntidadId { get; set; }
    public decimal Efectivo { get; set; }
    public decimal Especie { get; set; }
}

public class UpdateActxEntidadDto
{
    public int ActXEntidadId { get; set; }
    public int ActividadId { get; set; }
    public int EntidadId { get; set; }
    public decimal Efectivo { get; set; }
    public decimal Especie { get; set; }
}
