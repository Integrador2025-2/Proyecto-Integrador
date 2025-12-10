namespace Backend.Models.DTOs;

public class ObjetivoDto
{
    public int ObjetivoId { get; set; }
    public int ProyectoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string ResultadoEsperado { get; set; } = string.Empty;
    public string? ProyectoNombre { get; set; }
}

public class CreateObjetivoDto
{
    public int ProyectoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string ResultadoEsperado { get; set; } = string.Empty;
}

public class UpdateObjetivoDto
{
    public int ObjetivoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string ResultadoEsperado { get; set; } = string.Empty;
}
