namespace Backend.Models.DTOs;
public class EntidadDto
{
    public int EntidadId { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class CreateEntidadDto
{
    public string Nombre { get; set; } = string.Empty;
}

public class UpdateEntidadDto
{
    public int EntidadId { get; set; }
    public string Nombre { get; set; } = string.Empty;
}