namespace Backend.Models.DTOs;

public class ProyectoDto
{
    public int ProyectoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public string Estado { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
    public string? UsuarioNombre { get; set; }
}

public class CreateProyectoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Estado { get; set; } = "Activo";
    public int UsuarioId { get; set; }
}

public class UpdateProyectoDto
{
    public int ProyectoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}
