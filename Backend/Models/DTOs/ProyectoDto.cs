namespace Backend.Models.DTOs;
public class ProyectoDto
{
    public int ProyectoId { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int UsuarioId { get; set; }
}

public class CreateProyectoDto
{
    // For now only UsuarioId is required to create a Proyecto
    public int UsuarioId { get; set; }
}

public class UpdateProyectoDto
{
    public int ProyectoId { get; set; }
    public int UsuarioId { get; set; }
}
